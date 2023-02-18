using BO;

namespace BlApi;
public interface IProduct
{
    /// <summary>
    /// Asking for the list of products
    /// </summary>
    /// <returns>returning ProductForList</returns>
    /// 
   public IEnumerable<BO.ProductForList> GetListOfProducts(int x=0);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerable<BO.ProductItem> GetCatalog(int numOfCategory=0, IEnumerable<OrderItem?>? items=null);

    /// <summary>
    /// Product details request by ID
    /// </summary>
    /// <param name="idProduct"></param>
    /// <returns>Returns the requested product</returns>
    public BO.Product GetProduct(int idProduct);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idProduct"></param>
    /// <param name="cart"></param>
    /// <returns></returns>
    public BO.ProductItem GetProduct(int idProduct,BO.Cart cart);

    /// <summary>
    /// Adds a product to the data layer
    /// </summary>
    /// <param name="product">Product to add</param>
    public void AddProduct(BO.Product product);

    /// <summary>
    /// Product deletion
    /// </summary>
    /// <param name="idProduct">ID to delete a product</param>
    public void DeleteProduct(int idProduct);

    /// <summary>
    /// Update product data
    /// </summary>
    /// <param name="product">Product to be updated</param>
    public void UpDateProduct(BO.Product product);

    public IEnumerable<BO.ProductForList?> PopularItems();

}
