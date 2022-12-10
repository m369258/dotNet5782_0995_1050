using BO;
using Do;

namespace BlImplementation;
internal class Product : BlApi.IProduct
{
    DalApi.IDal myDal = new Dal.DalList();
    public delegate bool conditionFunction(Product entity);
    IEnumerable<BO.ProductForList> BlApi.IProduct.GetListOfProducts()
    {
        
        //Request from the data layer of all products
        IEnumerable<Do.Product> doProducts = myDal.product.GetAll();

        //Building a list of a logical layer for all products and returning it
        List<BO.ProductForList> ListProducts = new List<BO.ProductForList>();
        foreach (var item in doProducts)
        {
            BO.ProductForList product = new BO.ProductForList
            {
                ID = item.ID,
                Name = item.Name,
                Price = item.Price,
                category = (BO.Category)(item.Category)
            };
            ListProducts.Add(product);
        }
        return ListProducts;
    }

    public BO.Product GetProduct(int idProduct)
    {
        //Checking whether the ID is negative
        if (idProduct > 0)
        {
            //A product request based on the data layer identifier, if the information has not arrived, will thrthrow an error
            Do.Product p1;
            try { p1 = myDal.product.Get(idProduct); }
            catch(Do.DalDoesNotExistException ex)
            {
                throw new BO.InternalErrorException("this id doesnt exsist",ex); }

            //Build a layer product based on the data
            BO.Product product = new BO.Product()
            {
                ID = p1.ID,
                Name = p1.Name,
                Price = p1.Price,
                InStock = p1.InStock,
                Category = (BO.Category)(p1.Category),
            };

            return product;
        }
        else throw new BO.InvalidArgumentException("Negative ID");
    }


    public BO.ProductItem GetProduct(int idProduct, BO.Cart cart)
    {

        //Checking whether the ID is negative
        if (idProduct > 0)
        {
            bool isExsist = false;
            BO.OrderItem boOrderItem = new BO.OrderItem();

            //A product request based on the data layer identifier, if the information has not arrived, will throw an error
            Do.Product p1;
            try { p1 = myDal.product.Get(idProduct); }
            catch(Do.DalDoesNotExistException ex) { throw new InternalErrorException("this id doesnt exsist",ex); }

            //Going through all the items in the basket and saving the desired item
            for (int i = 0; i < cart.items.Count && !isExsist; i++)
            {
                if (cart.items[i].ProductId == idProduct)
                {
                    boOrderItem = cart.items[i];
                    isExsist = true;
                }
            }

            //If the item does not exist, a temporary error will be thrown
            if (!isExsist)
                throw new BO.InternalErrorException("The requested product does not exist in the shopping cart");

            //Building a logical product based on the data and returning it
            BO.ProductItem product = new BO.ProductItem
            {
                ProductID = p1.ID,
                Name = p1.Name,
                Price = p1.Price,
                Category = (BO.Category)(p1.Category),
                InStock = p1.InStock > 0 ? true : false,
                Amount = boOrderItem.QuantityPerItem
            };
            return product;
        }
        else throw new BO.InvalidArgumentException("Negative ID");
    }

    public void AddProduct(BO.Product product)
    {
        //Building a data product based on a logical product+Validity checks and appropriate error throwing
        Do.Product p1 = new Do.Product
        {
            ID = product.ID > 0 ? product.ID : throw new BO.InvalidArgumentException("Negative ID"),
            Name = product.Name != null ? product.Name : throw new BO.InvalidArgumentException("Empty name"),
            Price = product.Price >= 0 ? product.Price : throw new BO.InvalidArgumentException("Negative price"),
            InStock = product.InStock > 0 ? product.InStock : throw new BO.InvalidArgumentException("Unfavorable inStock"),
            Category = (Do.Category)(product.Category)
        };

        //Adding the data layer product
        myDal.product.Add(p1);
    }


    public void DeleteProduct(int idProduct)
    {
        //Request from the data layer of all orders
        IEnumerable<Do.Order> orders = myDal.order.GetAll();

        //A request for each order's details according to the identifier if the product intended for deletion exists in one of the orders will throw an exception
        IEnumerable<Do.OrderItem> orderItems = new List<Do.OrderItem>();
        foreach (var order in orders)
        {
            orderItems = myDal.orderItems.GetByIdOrder(order.ID);
            foreach (var item in orderItems)
            {

                if (item.ProductId == idProduct)
                    throw new BO.InternalErrorException("A used product cannot be deleted");
            }
        }
        //A request to delete a product according to the identifier and the product does not exist in the data will throw an error
        try { myDal.product.Delete(idProduct); }
        catch(Do.DalDoesNotExistException ex) {throw new InternalErrorException("A non-existent product cannot be deleted",ex); }

    }

    public void UpDateProduct(BO.Product product)
    {
        //A product request based on the data layer identifier, if the information has not arrived, will throw an error
        Do.Product p;
        try { p = myDal.product.Get(product.ID); }
        catch(Do.DalDoesNotExistException ex) { throw new InternalErrorException("this id doesnt exsist",ex); }

        //Updating the data layer product according to the received data in case of incorrect data will throw an error
        p.ID = product.ID > 0 ? product.ID : throw new BO.InvalidArgumentException("Negative ID");
        p.Name = product.Name != null ? product.Name : throw new BO.InvalidArgumentException("Empty name");
        p.Price = product.Price >= 0 ? product.Price : throw new BO.InvalidArgumentException("Negative price");
        p.InStock = product.InStock > 0 ? product.InStock : throw new BO.InvalidArgumentException("Unfavorable inStock");
        p.Category = (Do.Category)(product.Category);
        myDal.product.Update(p);

    }
}
