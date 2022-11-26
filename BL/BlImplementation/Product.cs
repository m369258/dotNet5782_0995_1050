using BlApi;
using BO;
namespace BlImplementation;

internal class Product : BlApi.IProduct
{
    // DalApi.IDal dalProduct = new Dal.DalList1();
    public DalApi.IDal myDal { get; set; }
    IEnumerable<BO.ProductForList> IProduct.GetListOfProducts()
    {
        List<BO.ProductForList> ListProducts = new List<BO.ProductForList>();
        foreach (var item in myDal.product.GetAll())
        {
            BO.ProductForList product = new BO.ProductForList();
            product.ID = item.ID;
            product.Name = item.Name;
            product.Price = item.Price;
            product.category = (BO.Category)(item.Category);
            ListProducts.Add(product);
        }
        return ListProducts;
    }

    public BO.Product GetProduct(int idProduct)
    {
        if (idProduct > 0)
        {
            try
            {
                BO.Product product = new BO.Product();
                Do.Product p1 = myDal.product.Get(idProduct);
                product.ID = p1.ID;
                product.Name = p1.Name;
                product.Price = p1.Price;
                product.InStock = p1.InStock;
                product.Category = (BO.Category)(p1.Category);
                return product;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        else
            throw new IncorrectIndex("Negative ID");
    }


    public BO.ProductItem GetProduct(int idProduct, BO.Cart cart)
    {
        if (idProduct > 0)
        {
            try
            {
                BO.ProductItem product = new BO.ProductItem();
                Do.Product p1 = myDal.product.Get(idProduct);
                product.ProductID = p1.ID;
                product.Price = p1.Price;
                product.Name = p1.Name;
                product.Category = (BO.Category)(p1.Category);
                product.InStock = p1.InStock > 0 ? true : false;
                product.Amount = p1.InStock;
                return product;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        else
            throw new IncorrectIndex("Negative ID");
    }

    public void AddProduct(BO.Product product)
    {
        Do.Product p1=new Do.Product();
        p1.ID = product.ID>0?product.ID:throw new IncorrectIndex("");
        p1.Name = product.Name!=null? product.Name:throw new InvalidField("Empty name"); 
        p1.Price = product.Price>=0? product.Price: throw new InvalidField("Negative price");
        p1.InStock=product.InStock>0? product.InStock: throw new InvalidField("Unfavorable inStock");
        p1.Category=(Do.Category)(product.Category);
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
        IEnumerable<Do.Order> orders=new List<Do.Order>();
        orders = myDal.order.GetAll();
        foreach (var order in orders)
        {
            IEnumerable<Do.OrderItem> orderItems=new List<Do.OrderItem>();
            orderItems = myDal.orderItems.GetByIdOrder(order.ID);
            foreach(var item in orderItems)
            {
                if (item.ProductId == idProduct)
                    throw new CannotPerformThisOperation("A used product cannot be deleted");
            }
        }

    }

}
