using BO;
namespace BlImplementation;

internal class Product : BlApi.IProduct
{
    DalApi.IDal myDal = DalApi.Factory.Get();

    public delegate bool conditionFunction(Product entity);
    IEnumerable<BO.ProductForList> BlApi.IProduct.GetListOfProducts(int numCategory)
    {
        IEnumerable<Do.Product?> doProducts;
        //A request to the data layer to fetch all the products, if a category exists, only the products of that category will be requested, otherwise all the products will be requested
        if (numCategory != 0)
            doProducts = myDal.product.GetAll(item => ((item)?.Category) == ((Do.Category)numCategory));
        else
            doProducts = myDal.product.GetAll();

        //Goes over all data layer products if the product does not exist it will throw an error.
        //therwise for each product a product will be built into a view layer list.
        return doProducts.Select(item => (item == null) ? throw new BO.InternalErrorException("Data layer item does not exist") : new BO.ProductForList()
        {
            ID = ((Do.Product)item!).ID,
            Name = ((Do.Product)item!).Name,
            Price = ((Do.Product)item!).Price,
            category = (BO.Category)(((Do.Product)item!).Category)!
            //Img = 
        });
    }

    public BO.Product GetProduct(int idProduct)
    {
        //Checking whether the ID is negative
        if (idProduct > 0)
        {
            //A product request based on the data layer identifier, if the information has not arrived, will thrthrow an error
            Do.Product? p1;
            try { p1 = myDal.product.Get(item => item?.ID == idProduct); }
            catch (Do.DalDoesNotExistException ex) { throw new BO.InternalErrorException("this id doesnt exsist", ex); }

            //Building a new object from the display product type
            BO.Product newP = new BO.Product();

            //Sending to an extension function that converts an object to be of type display layer.
            p1?.CopyBetweenEnriries(newP);

            //Copying mismatched fields
            newP.Category = (BO.Category)(p1?.Category)!;
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
            Do.Product p1;
            try { p1 = myDal.product.Get(item => item?.ID == idProduct); }
            catch (Do.DalDoesNotExistException ex) { throw new InternalErrorException("this id doesnt exsist", ex); }

            //Going through all the items in the basket and saving the desired item
            boOrderItem = cart.items?.FirstOrDefault(item => item?.ProductId == idProduct);

            if (boOrderItem == null)
                throw new BO.InternalErrorException("The requested product does not exist in the shopping cart");

            //Building a logical product based on the data and returning it
            BO.ProductItem newP = new BO.ProductItem();
            p1.CopyBetweenEnriries(newP);
            newP.Category = (BO.Category)(p1.Category!);
            newP.InStock = p1.InStock > 0 ? true : false;
            newP.Amount = boOrderItem!.QuantityPerItem;
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
            Category = (Do.Category)(product.Category ?? throw new BO.InvalidArgumentException("No category received"))
        });
    }


    public void DeleteProduct(int idProduct)
    {
        //Request from the data layer of all orders
        IEnumerable<Do.Order?> orders = myDal.order.GetAll();

        //The variable will receive a true value if in one of the orders (the details in the order) the product used otherwise will receive a false.
        bool isExsistProduct = orders.Any(currenOrder => myDal.orderItems.GetAll(x => x?.OrderId == (currenOrder?.ID ?? throw new BO.InternalErrorException("idProduct isnt exsist"))).Any(item => item?.ProductId == idProduct));

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
        try { p = myDal.product.Get(item => item?.ID == product.ID); }
        catch (Do.DalDoesNotExistException ex) { throw new BO.InternalErrorException("this id doesnt exsist", ex); }

        //Updating the data layer product according to the received data in case of incorrect data will throw an error
        p.ID = product.ID > 0 ? product.ID : throw new BO.InvalidArgumentException("Negative ID"); ;
        p.Name = product.Name != null ? product.Name : throw new BO.InvalidArgumentException("Empty name");
        p.Price = product.Price >= 0 ? product.Price : throw new BO.InvalidArgumentException("Negative price");
        p.InStock = product.InStock > 0 ? product.InStock : throw new BO.InvalidArgumentException("Unfavorable inStock");
        p.Category = (Do.Category)(product.Category ?? throw new BO.InvalidArgumentException("No category received"));
        myDal.product.Update(p);

    }

    public IEnumerable<BO.ProductItem> GetCatalog(int numCategory,IEnumerable<BO.OrderItem?>? items)
    {
        IEnumerable<Do.Product?> doProducts;
        if (numCategory != 0)
            doProducts = myDal.product.GetAll(item => ((item)?.Category) == ((Do.Category)numCategory));
        else
            doProducts = myDal.product.GetAll();

        var result = from item in doProducts
                     let amount = items?.FirstOrDefault(it => it?.ProductId == ((Do.Product)item!).ID)?.QuantityPerItem
                     select new BO.ProductItem
                     {
                             ProductID = ((Do.Product)item!).ID,
                             Name = ((Do.Product)item!).Name,
                             Price = ((Do.Product)item!).Price,
                             Category = (BO.Category)(((Do.Product)item!).Category)!,
                             InStock = ((Do.Product)item!).InStock > 0 ? true : false,
                             Amount = amount ?? 0  
                     };
        return result;
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    }
}
