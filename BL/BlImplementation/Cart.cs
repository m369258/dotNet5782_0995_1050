using BO;
using Do;
namespace BlImplementation;

internal class Cart : BlApi.ICart
{
    //Request access to the data layer
    DalApi.IDal myDal = new Dal.DalList();
    public BO.Cart Add(BO.Cart myCart, int idProduct)
    {
        BO.OrderItem myOrderItem = new BO.OrderItem();
        
        //Checking whether the product is in the shopping basket
       BO.OrderItem? boOrderItem= myCart.items?.FirstOrDefault(currItem => currItem?.ProductId == idProduct);

        //If a product does not exist in the shopping basket
        if (boOrderItem==null)
        {
            //Request a data layer product
            Do.Product doProduct;
            try { doProduct = myDal.product.Get(item=>item?.ID==idProduct); }
            catch (Do.DalDoesNotExistException ex) { throw new InternalErrorException("This item does not exist", ex); }//- may have been removed

            //Checking if this item is in stock
            if (doProduct.InStock >= 1)
            {
                //Creating an orderItem and adding it to the list of details in the basket
                myOrderItem.ProductId = idProduct;
                myOrderItem.NameProduct = doProduct.Name;
                myOrderItem.productPrice = doProduct.Price;
                myOrderItem.QuantityPerItem = 1;
                myOrderItem.productPrice = doProduct.Price;
                myOrderItem.TotalPrice = doProduct.Price;
                //adding it to the list of details in the basket
                myCart.items?.Add(myOrderItem);
                myCart.TotalPrice += doProduct.Price;
            }
            else throw new NotEnoughInStockException(doProduct.ID, doProduct.Name??"" , "There is not enough stock of this product.");
        }
        else//if the product is in the shopping basket:
        {
            int productId = boOrderItem!.ProductId;

            //Request a data layer product
            Do.Product myProduct;
            try { myProduct = myDal.product.Get(item => item?.ID == idProduct); }
            catch (Do.DalDoesNotExistException ex) { throw new InternalErrorException("This item does not exist", ex); }

            //Checking whether the desired quantity is in stock
            if (boOrderItem!.QuantityPerItem <= myProduct.InStock)
            {
                boOrderItem!.QuantityPerItem++;
                boOrderItem!.productPrice = myProduct.Price;
                boOrderItem!.TotalPrice += myProduct.Price;
                myCart.TotalPrice += myProduct.Price;
            }
            else throw new BO.NotEnoughInStockException(productId, myProduct.Name ?? throw new Exception(), "There is not enough stock of this product.");
        }
        return myCart;
    }

    public void MakeAnOrder(BO.Cart myCart)
    {
        //In case the customer's name or address is empty, an error will be thrown
        if(myCart.CustomerName=="") 
              throw new BO.InvalidArgumentException("empty customer name");
        if (myCart.CustomerAddress =="")
            throw new BO.InvalidArgumentException("empty customer address");
        //In case the email address is not correct
        if (!myCart.CustomerEmail!.Contains("@") || !myCart.CustomerEmail.Contains("."))
            throw new BO.InvalidArgumentException("Invalid email");

        //If there are no items in the basket, an error will be thrown
        _ = myCart.items ?? throw new BO.InternalErrorException("there arent items in the cart");

        //checking that all products exist
        myCart.items.All(item => normalityByProductInTheBasket(item));

        //Create an order object (data entity) based on the data in the basket
        Do.Order myOrder = new Do.Order();
        myCart.CopyBetweenEnriries(myOrder);
        myOrder.OrderDate = DateTime.Now;
        myOrder.ShipDate = null;
        myOrder.DeliveryDate = null;

        //Attempt a request to add a created order (data entity) to the data layer and you will get back an order number
        int orderId = myDal.order.Add(myOrder);

        //Build objects of an item in the order (data entity) according to the data from the basket and the aforementioned order number
        myCart.items.All(item => makeOrder(item,orderId)); 
    }

    
    public BO.Cart Update(BO.Cart myCart, int idProduct, int newQuantity)
    {
        int productId;
        //In case of a negative amount an exception will be thrown
        if (newQuantity < 0)
            throw new BO.InvalidInputException("Quantity cannot be a negative number");

        //Returns the item in the first order that has the given product ID
        BO.OrderItem? boOrderItem=  myCart.items?.FirstOrDefault(currenItem => currenItem?.ProductId == idProduct);

        //In case the item is in the basket we will try to update the quantity for this item
        if (boOrderItem!=null)
        {
            //In case the desired quantity increases, we will check if it is in stock.
            if (newQuantity > boOrderItem.QuantityPerItem)
            {
                productId = boOrderItem.ProductId;
                Do.Product myProduct;
                try { myProduct = myDal.product.Get(item => item?.ID ==productId); }
                catch (Do.DalDoesNotExistException ex) { throw new BO.InternalErrorException("this id product is not exsist", ex); }
                //we will check if it is in stock and we will update the details
                if (newQuantity <= myProduct.InStock)
                {
                    myCart.TotalPrice += (newQuantity - boOrderItem.QuantityPerItem) * boOrderItem.productPrice;
                    boOrderItem.QuantityPerItem = newQuantity;
                    boOrderItem.TotalPrice = boOrderItem.productPrice * newQuantity;
                }
                else throw new BO.NotEnoughInStockException(myProduct.ID , myProduct.Name ?? throw new Exception(), "There is not enough stock of this product.");
            }
            //In case the desired quantity is small, we will check if it is still in stock.
            else if (newQuantity < boOrderItem.QuantityPerItem && newQuantity != 0)
            {
                productId = boOrderItem.ProductId;
                Do.Product myProduct;
                try { myProduct = myDal.product.Get(item => item?.ID ==productId); }
                catch (Do.DalDoesNotExistException ex) { throw new InternalErrorException("this id product is not exsist", ex); }

                //check if it is still in stock and update the details
                if (newQuantity <= myProduct.InStock)
                {
                    myCart.TotalPrice -= (boOrderItem.QuantityPerItem - newQuantity) * boOrderItem.productPrice;
                    boOrderItem.QuantityPerItem = newQuantity;
                    boOrderItem.TotalPrice = boOrderItem.productPrice * newQuantity;
                }
            }
            //If the quantity became 0 - delete the corresponding item from the basket and update the total price of the shopping basket
            else
            {
                myCart.TotalPrice -= boOrderItem.TotalPrice;
                myCart.items?.Remove(boOrderItem);
            }
        }
        else//If it does not exist, the item will be added to the cart
        {
            this.add(myCart, idProduct, newQuantity);
        }

        return myCart;
    }

    //Local helper functions:

    /// <summary>
    /// A private function of the department that adds a product to the shopping cart according to the desired quantity for this item
    /// </summary>
    /// <param name="myCart">the shopping cart</param>
    /// <param name="idProduct">the id of the product to add</param>
    /// <param name="newQuantity">quantity of the product</param>
    /// <exception cref="NotEnoughInStockException">There is not enough stock of this product.</exception>
    private void add(BO.Cart myCart, int idProduct, int newQuantity)
    {
        Do.Product myProduct;
        try { myProduct = myDal.product.Get(item => item?.ID == idProduct); }
        catch (Do.DalDoesNotExistException ex) { throw new BO.InternalErrorException("this id product is not exsist", ex); }
        //Checking whether the desired quantity of this item is in stock
        BO.OrderItem myOrderItem = new BO.OrderItem();

        if (myProduct.InStock >= newQuantity)
        {
            //Outputting an item and initializing its values
            myOrderItem.ProductId = idProduct;
            myOrderItem.NameProduct = myProduct.Name;
            myOrderItem.productPrice = myProduct.Price;
            myOrderItem.QuantityPerItem = newQuantity;
            myOrderItem.productPrice = myProduct.Price;
            myOrderItem.TotalPrice = (myProduct.Price) * newQuantity;
            //Adding an item to the list of items
            myCart.items!.Add(myOrderItem);
            myCart.TotalPrice += (myProduct.Price) * newQuantity;
        }
        else throw new BO.NotEnoughInStockException(myProduct.ID, myProduct.Name ?? "", "There is not enough stock of this product.");
    }

    /// <summary>
    /// Checks that all the desired products for the order exist
    /// </summary>
    /// <param name="boOrderItem">orderItem-logical layer</param>
    /// <returns></returns>
    /// <exception cref="BO.InternalErrorException">in case idProduct isnt exsust</exception>
    /// <exception cref="BO.InvalidArgumentException">negative quantity</exception>
    /// <exception cref="BO.NotEnoughInStockException">No quantity in stock</exception>

    private bool normalityByProductInTheBasket(BO.OrderItem? boOrderItem)
    {
       int productId = boOrderItem!.ProductId;
        Do.Product myProduct;
        // check if it is in stock
        try { myProduct = myDal.product.Get(item => item?.ID == productId); }
        catch (Do.DalDoesNotExistException ex) { throw new BO.InternalErrorException("this product id is not exists", ex); }
        //Checking whether the quantities are positive
        if (boOrderItem.QuantityPerItem < 0)
            throw new BO.InvalidArgumentException("negative quantity");
        //Not enough in stock
        if (boOrderItem.QuantityPerItem > myProduct.InStock)
            throw new BO.NotEnoughInStockException(productId,"No quantity in stock");
        //In case we have changed the price of the item, we will also update the price in the basket
        if (myProduct.Price != boOrderItem.productPrice)
            boOrderItem.productPrice = myProduct.Price;
        return true;
    }

    /// <summary>
    /// Place an order by adding order items to the data layer and update the ordered product inventory
    /// </summary>
    /// <param name="boOrderItem">orderItem-logical layer</param>
    /// <param name="orderId">ID order</param>
    /// <returns></returns>
    /// <exception cref="BO.InternalErrorException"></exception>
    private bool makeOrder(BO.OrderItem? boOrderItem,int orderId)
    {
        Do.OrderItem myOrderItem = new Do.OrderItem
        {
            OrderId = orderId,
            ProductId = boOrderItem!.ProductId,
            Price = boOrderItem.productPrice,
            Amount = boOrderItem.QuantityPerItem
        };

        //and make attempts to request the addition of an order item
        try { myDal.orderItems.Add(myOrderItem); }
        catch (DalAlreadyExistsException ex) { throw new BO.InternalErrorException("Unable to add", ex); }

        Do.Product product;
        try { product = myDal.product.Get(item => item?.ID == myOrderItem.ProductId); }
        catch (DalAlreadyExistsException ex) { throw new BO.InternalErrorException("The item already exists", ex); }

        //Requests to update these products after the inventory is updated
        product.InStock -= myOrderItem.Amount;
        myDal.product.Update(product);

        return true;
    }

}
