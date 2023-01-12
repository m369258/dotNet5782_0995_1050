using BO;
namespace BlImplementation;

internal class Cart : BlApi.ICart
{
    //Request access to the data layer
    DalApi.IDal myDal = DalApi.Factory.Get();
    public BO.Cart Add(BO.Cart cart, int idProduct)
    {
       BO.Cart myCart = new BO.Cart();
        //cart.CopyBetweenEnriries(myCart);
        cart.CopyBetweenEnriries(myCart);
        BO.OrderItem myOrderItem = new BO.OrderItem();

        //Checking whether the product is in the shopping basket
        BO.OrderItem? boOrderItem = myCart.items?.FirstOrDefault(currItem => currItem?.ProductId == idProduct);

        //If a product does not exist in the shopping basket
        if (boOrderItem == null)
        {
            //Request a data layer product
            Do.Product doProduct;
            try { doProduct = myDal.product.Get(item => item?.ID == idProduct); }
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
                if(myCart.items==null)
                    myCart.items = new List<OrderItem?>();
                
                myCart.items?.Add(myOrderItem);
                
                myCart.TotalPrice += doProduct.Price;
            }
            else throw new NotEnoughInStockException(doProduct.ID, doProduct.Name ?? "", "There is not enough stock of this product.");
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
        if (myCart.CustomerName == "")
            throw new BO.InvalidArgumentException("empty customer name");
        if (myCart.CustomerAddress == "")
            throw new BO.InvalidArgumentException("empty customer address");
        //In case the email address is not correct
        if (!myCart.CustomerEmail!.Contains("@") || !myCart.CustomerEmail.Contains("."))
            throw new BO.InvalidArgumentException("Invalid email");

        //If there are no items in the basket, an error will be thrown
        _ = myCart.items ?? throw new BO.InternalErrorException("there arent items in the cart");

        int idOrder = myDal.order.Add(
                               new Do.Order
                               {
                                   CustomerName = myCart.CustomerName,
                                   CustomerAddress = myCart.CustomerAddress,
                                   CustomerEmail = myCart.CustomerEmail,
                                   OrderDate = DateTime.Now,
                                   ShipDate = null,
                                   DeliveryDate = null
                               });

        IEnumerable<Do.Product?> products = myDal.product.GetAll();

        var result = from item in myCart.items
                     join product in products on item.ProductId equals product?.ID into temporary
                     from temp in temporary.DefaultIfEmpty()
                     select new
                     {
                         orderItem = myDal.orderItems.Add(
                         new Do.OrderItem
                         {
                             OrderId = idOrder,
                             ProductId = item?.ProductId ?? 0,
                             Amount = item?.QuantityPerItem > 0 ? item?.QuantityPerItem ?? 0 : throw new BO.InvalidArgumentException("negative quantity"),
                             Price = item?.TotalPrice ?? 0,  
                         }),
                         product = new Do.Product
                         {
                             ID = temp == null ? throw new BO.InternalErrorException("product does not exist") : item?.ProductId ?? 0,
                             Price = temp?.Price ?? 0,
                             Category = temp?.Category ?? 0,
                             InStock = temp?.InStock > item?.QuantityPerItem ? temp?.InStock - item?.QuantityPerItem ?? 0 : throw new BO.NotEnoughInStockException(item.ProductId, "No quantity in stock"),
                             Name = temp?.Name
                         }
                     };

        result.ToList().ForEach(res => myDal.product.Update(res.product));
    }

    public BO.Cart Update(BO.Cart myCart, int idProduct, int newQuantity)
    {
        int productId;
        //In case of a negative amount an exception will be thrown
        if (newQuantity < 0)
            throw new BO.InvalidInputException("Quantity cannot be a negative number");

        //Returns the item in the first order that has the given product ID
        BO.OrderItem? boOrderItem = myCart.items?.FirstOrDefault(currenItem => currenItem?.ProductId == idProduct);

        //In case the item is in the basket we will try to update the quantity for this item
        if (boOrderItem != null)
        {
            //In case the desired quantity increases, we will check if it is in stock.
            if (newQuantity > boOrderItem.QuantityPerItem)
            {
                productId = boOrderItem.ProductId;
                Do.Product myProduct;
                try { myProduct = myDal.product.Get(item => item?.ID == productId); }
                catch (Do.DalDoesNotExistException ex) { throw new BO.InternalErrorException("this id product is not exsist", ex); }
                //we will check if it is in stock and we will update the details
                if (newQuantity <= myProduct.InStock)
                {
                    myCart.TotalPrice += (newQuantity - boOrderItem.QuantityPerItem) * boOrderItem.productPrice;
                    boOrderItem.QuantityPerItem = newQuantity;
                    boOrderItem.TotalPrice = boOrderItem.productPrice * newQuantity;
                }
                else throw new BO.NotEnoughInStockException(myProduct.ID, myProduct.Name ?? throw new Exception(), "There is not enough stock of this product.");
            }
            //In case the desired quantity is small, we will check if it is still in stock.
            else if (newQuantity < boOrderItem.QuantityPerItem && newQuantity != 0)
            {
                productId = boOrderItem.ProductId;
                Do.Product myProduct;
                try { myProduct = myDal.product.Get(item => item?.ID == productId); }
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
}