
using BlApi;
using BO;
using DalApi;
using Do;

namespace BlImplementation;

internal class Cart : BlApi.ICart
{
    //Request access to the data layer
    DalApi.IDal MyDal = new Dal.DalList();
    public BO.Cart Add(BO.Cart myCart, int idProduct)
    {
        BO.OrderItem myOrderItem = new BO.OrderItem();
        bool isExist = false;
        int i;
        //Checking whether the product is in the shopping basket
        for (i = 0; i < myCart.items.Count && !isExist; i++)
        {
            if (myCart.items[i].ProductId == idProduct)
                isExist = true;
        }

        //If a product does not exist in the shopping basket
        if (!isExist)
        {
            Do.Product? myProduct;
            try { myProduct = MyDal.product.Get(idProduct); }
            catch(Do.DalDoesNotExistException ex) { throw new InternalErrorException("This item does not exist",ex); }//- may have been removed
            //Checking if this item is in stock
            if (myProduct?.InStock >= 1)
            {
                //Creating an orderItem and adding it to the list of details in the basket
                myOrderItem.ProductId = idProduct;
                myOrderItem.NameProduct = myProduct?.Name;
                myOrderItem.productPrice = myProduct?.Price ?? throw new Exception();
                myOrderItem.QuantityPerItem = 1;
                myOrderItem.productPrice = myProduct?.Price ?? throw new Exception();
                myOrderItem.TotalPrice = myProduct?.Price ?? throw new Exception();
                //adding it to the list of details in the basket
                myCart.items.Add(myOrderItem);
                myCart.TotalPrice += myProduct?.Price ?? throw new Exception();
            }
            else throw new NotEnoughInStockException(myProduct?.ID ?? throw new Exception(), myProduct?.Name ?? throw new Exception(), "There is not enough stock of this product.");
        }
        else//if the product is in the shopping basket:
        {
            int productId = myCart.items[i - 1].ProductId;
            Do.Product? myProduct;
            try { myProduct = MyDal.product.Get(productId); }
            catch(Do.DalDoesNotExistException ex) { throw new InternalErrorException("This item does not exist",ex); }

            //Checking whether the desired quantity is in stock
            if (myCart.items[i - 1].QuantityPerItem <= myProduct?.InStock)
            {
                myCart.items[i - 1].QuantityPerItem++;
                myCart.items[i - 1].productPrice = myProduct?.Price ?? throw new Exception();
                myCart.items[i - 1].TotalPrice += myProduct?.Price ?? throw new Exception();
                myCart.TotalPrice += myProduct?.Price ?? throw new Exception();
            }
            else throw new BO.NotEnoughInStockException(productId, myProduct?.Name ?? throw new Exception(), "There is not enough stock of this product.");
        }

        return myCart;
    }

    public void MakeAnOrder(BO.Cart myCart)
    {
        int productId;
        //In case the customer's name or address is empty, an error will be thrown
        if (myCart.CustomerName == null)
            throw new BO.InvalidArgumentException("empty customer name");
        if (myCart.CustomerAddress == null)
            throw new BO.InvalidArgumentException("empty customer address");
        //In case the email address is not correct
        if (!myCart.CustomerEmail.Contains("@") || !myCart.CustomerEmail.Contains("."))
            throw new BO.InvalidArgumentException("Invalid email");

        //checking that all products exist
        foreach (var item in myCart.items)
        {
            productId = item.ProductId;
            Do.Product? myProduct;
            // check if it is in stock
            try { myProduct = MyDal.product.Get(productId); }
            catch (Do.DalDoesNotExistException ex) { throw new BO.InternalErrorException("this product id is not exists",ex); }
            //Checking whether the quantities are positive
            if (item.QuantityPerItem < 0)
                throw new BO.InvalidArgumentException("negative quantity");
            //Not enough in stock
            if (item.QuantityPerItem > myProduct?.InStock)
                throw new BO.InternalErrorException("No quantity in stock");
            //In case we have changed the price of the item, we will also update the price in the basket
            if (myProduct?.Price != item.productPrice)
                item.productPrice = myProduct?.Price ?? throw new Exception();
        }

        //Create an order object (data entity) based on the data in the basket
        Do.Order myOrder = new Do.Order()
        {
            CustomerName = myCart.CustomerName,
            CustomerAddress = myCart.CustomerAddress,
            CustomerEmail = myCart.CustomerEmail,
            OrderDate = DateTime.Now,
            ShipDate = null,
            DeliveryDate = null,
        };

        //Attempt a request to add a created order (data entity) to the data layer and you will get back an order number
        int orderId = MyDal.order.Add(myOrder);

        //Build objects of an item in the order (data entity) according to the data from the basket and the aforementioned order number
        foreach (var item in myCart.items)
        {
            Do.OrderItem myOrderItem = new Do.OrderItem()
            {
                OrderId = orderId,
                ProductId = item.ProductId,
                Price = item.productPrice,
                Amount = item.QuantityPerItem
            };
            //and make attempts to request the addition of an order item
            try { MyDal.orderItems.Add(myOrderItem); }
            catch(DalAlreadyExistsException ex) { throw new InternalErrorException("Unable to add",ex); }

            Do.Product product;
            try { product = (Do.Product)(MyDal.product.Get(myOrderItem.ProductId)); }
            catch (DalAlreadyExistsException ex) { throw new InternalErrorException("The item already exists",ex); }

            //Requests to update these products after the inventory is updated
            product.InStock  -= myOrderItem.Amount;
            MyDal.product.Update(product);
        }
    }

    /// <summary>
    /// A private function of the department that adds a product to the shopping cart according to the desired quantity for this item
    /// </summary>
    /// <param name="myCart">the shopping cart</param>
    /// <param name="idProduct">the id of the product to add</param>
    /// <param name="newQuantity">quantity of the product</param>
    /// <exception cref="NotEnoughInStockException">There is not enough stock of this product.</exception>
    private void Add(BO.Cart myCart, int idProduct, int newQuantity)
    {
        Do.Product? myProduct;
        try { myProduct = MyDal.product.Get(idProduct); }
        catch(Do.DalDoesNotExistException ex) { throw new BO.InternalErrorException("this id product is not exsist",ex); }
        //Checking whether the desired quantity of this item is in stock
        BO.OrderItem myOrderItem = new BO.OrderItem();

        if (myProduct?.InStock >= newQuantity)
        {
            //Outputting an item and initializing its values
            myOrderItem.ProductId = idProduct;
            myOrderItem.NameProduct = myProduct?.Name;
            myOrderItem.productPrice = myProduct?.Price ?? throw new Exception();
            myOrderItem.QuantityPerItem = newQuantity;
            myOrderItem.productPrice = myProduct?.Price ?? throw new Exception();
            myOrderItem.TotalPrice = (myProduct?.Price?? throw new Exception()) * newQuantity;
            //Adding an item to the list of items
            myCart.items.Add(myOrderItem);
            myCart.TotalPrice += (myProduct?.Price ?? throw new Exception()) * newQuantity;
        }
        else throw new BO.NotEnoughInStockException(myProduct?.ID ?? throw new Exception() , myProduct?.Name ?? throw new Exception(), "There is not enough stock of this product.");
    }

    public BO.Cart Update(BO.Cart myCart, int idProduct, int newQuantity)
    {
        if (newQuantity < 0)
            throw new BO.InvalidInputException("Quantity cannot be a negative number");
        bool isExist = false;
        int i;
        //Checking whether the product is in the shopping basket
        for (i = 0; i < myCart.items.Count && !isExist; i++)
        {
            if (myCart.items[i].ProductId == idProduct)
                isExist = true;
        }

        //In case the item is in the basket we will try to update the quantity for this item
        if (isExist)
        {
            //In case the desired quantity increases, we will check if it is in stock.
            if (newQuantity > myCart.items[i - 1].QuantityPerItem)
            {
                int productId = myCart.items[i - 1].ProductId;
                Do.Product? myProduct;
                try { myProduct = MyDal.product.Get(productId); }
                catch(Do.DalDoesNotExistException ex) { throw new BO.InternalErrorException("this id product is not exsist",ex); }
                //we will check if it is in stock and we will update the details
                if (newQuantity <= myProduct?.InStock)
                {
                    myCart.TotalPrice += (newQuantity - myCart.items[i - 1].QuantityPerItem) * myCart.items[i - 1].productPrice;
                    myCart.items[i - 1].QuantityPerItem = newQuantity;
                    myCart.items[i - 1].TotalPrice = myCart.items[i - 1].productPrice * newQuantity;
                }

                else throw new BO.NotEnoughInStockException(myProduct?.ID ?? throw new Exception(), myProduct?.Name ?? throw new Exception(), "There is not enough stock of this product.");
            }
            //In case the desired quantity is small, we will check if it is still in stock.
            else if (newQuantity < myCart.items[i - 1].QuantityPerItem && newQuantity != 0)
            {
                int productId = myCart.items[i - 1].ProductId;
                Do.Product? myProduct;
                try { myProduct = MyDal.product.Get(productId); }
                catch(Do.DalDoesNotExistException ex) { throw new InternalErrorException("this id product is not exsist",ex); }

                //check if it is still in stock and update the details
                if (newQuantity <= myProduct?.InStock)
                {
                    myCart.TotalPrice -= (myCart.items[i - 1].QuantityPerItem - newQuantity) * myCart.items[i - 1].productPrice;
                    myCart.items[i - 1].QuantityPerItem = newQuantity;
                    myCart.items[i - 1].TotalPrice = myCart.items[i - 1].productPrice * newQuantity;
                }
            }
            //If the quantity became 0 - delete the corresponding item from the basket and update the total price of the shopping basket
            else
            {
                myCart.TotalPrice -= myCart.items[i - 1].TotalPrice;
                myCart.items.Remove(myCart.items[i - 1]);
            }
        }
        else//If it does not exist, the item will be added to the cart??אנחנו הוספנו על פי הגיון, זה בסדר??
        {
            Add(myCart, idProduct, newQuantity);
        }

        return myCart;
    }
}
