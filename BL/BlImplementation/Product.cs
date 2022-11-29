namespace BlImplementation;

internal class Product : BlApi.IProduct
{
     DalApi.IDal myDal = new Dal.DalList();
   // public DalApi.IDal myDal { get; set; }
    IEnumerable<BO.ProductForList> BlApi.IProduct.GetListOfProducts()
    {
        IEnumerable<Do.Product> doProducts = myDal.product.GetAll();
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
        if (idProduct > 0)
        {
            Do.Product p1;
            try { p1 = myDal.product.Get(idProduct); }
            catch (Exception ex) { throw (ex); }//!!לדאוג לזריקת שגיאה מתאימה
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
        else throw new BO.IncorrectIndex("Negative ID");
    }


    public BO.ProductItem GetProduct(int idProduct, BO.Cart cart)
    {
        if (idProduct > 0)
        {
            try
            {
                bool isExsist = false;
                BO.OrderItem boOrderItem = new BO.OrderItem();
                Do.Product p1 = myDal.product.Get(idProduct);
                for(int i=0;i<cart.items.Count;i++)
                {
                    if (cart.items[i].ProductId == idProduct)
                    {
                        boOrderItem = cart.items[i];
                        isExsist = true;
                    }
                }
                BO.ProductItem product = new BO.ProductItem
                {
                    ProductID = p1.ID,
                    Name = p1.Name,
                    Price = p1.Price,
                    Category = (BO.Category)(p1.Category),
                    InStock = p1.InStock > 0 ? true : false,
                    Amount=boOrderItem.QuantityPerItem
                };
                return product;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        else
            throw new BO.IncorrectIndex("Negative ID");
    }

    public void AddProduct(BO.Product product)
    {
        Do.Product p1 = new Do.Product
        {
            ID = product.ID > 0 ? product.ID : throw new BO.IncorrectIndex(""),
            Name = product.Name != null ? product.Name : throw new BO.InvalidField("Empty name"),
            Price = product.Price >= 0 ? product.Price : throw new BO.InvalidField("Negative price"),
            InStock = product.InStock > 0 ? product.InStock : throw new BO.InvalidField("Unfavorable inStock"),
            Category = (Do.Category)(product.Category)
        };
        try
        {
            myDal.product.Add(p1);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }


    public void DeleteProduct(int idProduct)
    {
        IEnumerable<Do.Order> orders = new List<Do.Order>();
        IEnumerable<Do.OrderItem> orderItems = new List<Do.OrderItem>();
        try
        {
            orders = myDal.order.GetAll();
            foreach (var order in orders)
            {
                orderItems = myDal.orderItems.GetByIdOrder(order.ID);
                foreach (var item in orderItems)
                {

                    if (item.ProductId == idProduct)
                        throw new BO.CannotPerformThisOperation("A used product cannot be deleted");
                }

            }

            myDal.product.Delete(idProduct);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    public void UpDateProduct(BO.Product product)
    {
        try
        {
            Do.Product p = myDal.product.Get(product.ID);
            p.ID = product.ID > 0 ? product.ID : throw new BO.IncorrectIndex("Negative ID");
            p.Name = product.Name != null ? product.Name : throw new BO.InvalidField("Empty name");
            p.Price = product.Price >= 0 ? product.Price : throw new BO.InvalidField("Negative price");
            p.InStock = product.InStock > 0 ? product.InStock : throw new BO.InvalidField("Unfavorable inStock");
            p.Category = (Do.Category)(product.Category);
            myDal.product.Update(p);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


}
