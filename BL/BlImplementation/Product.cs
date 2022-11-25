using BlApi;
using DalApi;

namespace BlImplementation;

internal class Product: BlApi.IProduct
{
    DalApi.IDal dalProduct = new Dal.DalList1();
    public IEnumerable<BO.ProductForList> GetListOfProducts()
    {
        List<BO.ProductForList> ListProducts = new List<BO.ProductForList>();
        foreach (var item in dalProduct.product.GetAll())
        {
            BO.ProductForList product = new BO.ProductForList();
            product.ID = item.ID;
            product.Name = item.Name;
            product.Price=item.Price;
           product.Category = (BO.Category)(item.Category);
            ListProducts.Add(product);
        }
        return ListProducts;
    }

}
