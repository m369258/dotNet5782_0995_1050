using BO;

namespace BlImplementation;
internal class Product : BlApi.IProduct
{
    DalApi.IDal myDal = new Dal.DalList();
    public delegate bool conditionFunction(Product entity);
    IEnumerable<BO.ProductForList> BlApi.IProduct.GetListOfProducts(int x)
    {
        IEnumerable<Do.Product?> doProducts;
        //Request from the data layer of all products
        if (x != 0)
            doProducts = myDal.product.GetAll(item => ((item)?.Category) == ((Do.Category)x));
        else
            doProducts = myDal.product.GetAll();

        //
        return doProducts.Select(item => new BO.ProductForList()
        {
            ID = item?.ID ?? throw new Exception(),
            Name = item?.Name ?? throw new Exception(),
            Price = item?.Price ?? throw new Exception(),
            category = (BO.Category)(item?.Category)
        });
    }

    public BO.Product GetProduct(int idProduct)
    {
        //Checking whether the ID is negative
        if (idProduct > 0)
        {
            //A product request based on the data layer identifier, if the information has not arrived, will thrthrow an error
            Do.Product? p1;
            try { p1 = myDal.product.Get(idProduct); }
            catch (Do.DalDoesNotExistException ex)
            {
                throw new BO.InternalErrorException("this id doesnt exsist", ex);
            }
            BO.Product newP = new BO.Product();
            p1?.CopyBetweenEnriries(newP);
            newP.Category = (BO.Category)(p1?.Category);
            return newP;
        }
        else throw new BO.InvalidArgumentException("Negative ID");
    }


    public BO.ProductItem GetProduct(int idProduct, BO.Cart cart)
    {

        //Checking whether the ID is negative
        if (idProduct > 0)
        {
            BO.OrderItem? boOrderItem = new BO.OrderItem();

            //A product request based on the data layer identifier, if the information has not arrived, will throw an error
            Do.Product? p1;
            try { p1 = myDal.product.Get(idProduct); }
            catch (Do.DalDoesNotExistException ex) { throw new InternalErrorException("this id doesnt exsist", ex); }

            //Going through all the items in the basket and saving the desired item

            boOrderItem = cart.items.FirstOrDefault(item => item.ProductId == idProduct);

            if (boOrderItem == null)
                throw new BO.InternalErrorException("The requested product does not exist in the shopping cart");

            //Building a logical product based on the data and returning it
            BO.ProductItem newP = new BO.ProductItem();
            p1?.CopyBetweenEnriries(newP);
            newP.Category = (BO.Category)(p1?.Category);
            newP.InStock = p1?.InStock > 0 ? true : false;
            newP.Amount = boOrderItem.QuantityPerItem;
            return newP;
        }
        else throw new BO.InvalidArgumentException("Negative ID");
    }

    public void AddProduct(BO.Product product)
    {
        //Building a data product based on a logical product+Validity checks and appropriate error throwing
        myDal.product.Add(new Do.Product
        {
            ID = product.ID > 0 ? product.ID : throw new BO.InvalidArgumentException("Negative ID"),
            Name = product.Name != null ? product.Name : throw new BO.InvalidArgumentException("Empty name"),
            Price = product.Price >= 0 ? product.Price : throw new BO.InvalidArgumentException("Negative price"),
            InStock = product.InStock > 0 ? product.InStock : throw new BO.InvalidArgumentException("Unfavorable inStock"),
            Category = (Do.Category)(product.Category)
        });
    }


    public void DeleteProduct(int idProduct)
    {
        //Request from the data layer of all orders
        IEnumerable<Do.Order?> orders = myDal.order.GetAll();

        //The variable will receive a true value if in one of the orders (the details in the order) the product used otherwise will receive a false.
        bool isExsistProduct = orders.Any(currenOrder => myDal.orderItems.GetByIdOrder(currenOrder?.ID ?? throw new Exception()).Any(item => item?.ProductId == idProduct));
        //If the product is used, an error will be thrown
        if (isExsistProduct)
            throw new BO.InternalErrorException("A used product cannot be deleted");

        //A request to delete a product according to the identifier and the product does not exist in the data will throw an error
        try { myDal.product.Delete(idProduct); }
        catch (Do.DalDoesNotExistException ex) { throw new InternalErrorException("A non-existent product cannot be deleted", ex); }
    }

    public void UpDateProduct(BO.Product product)
    {
        //A product request based on the data layer identifier, if the information has not arrived, will throw an error
        Do.Product p;
        try { p = myDal.product.Get(product.ID); }
        catch (Do.DalDoesNotExistException ex) { throw new InternalErrorException("this id doesnt exsist", ex); }

        //Updating the data layer product according to the received data in case of incorrect data will throw an error
        p.ID = product.ID > 0 ? product.ID : throw new BO.InvalidArgumentException("Negative ID"); ;
        p.Name = product.Name != null ? product.Name : throw new BO.InvalidArgumentException("Empty name");
        p.Price = product.Price >= 0 ? product.Price : throw new BO.InvalidArgumentException("Negative price");
        p.InStock = product.InStock > 0 ? product.InStock : throw new BO.InvalidArgumentException("Unfavorable inStock");
        p.Category = (Do.Category)(product.Category);
        myDal.product.Update(p);

    }

}
