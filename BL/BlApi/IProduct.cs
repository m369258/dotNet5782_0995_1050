namespace BlApi;
public interface IProduct
{
    /// <summary>
    /// Asking for the list of products
    /// </summary>
    /// <returns>returning ProductForList</returns>
    public IEnumerable<BO.ProductForList> GetListOfProducts();
    


}
