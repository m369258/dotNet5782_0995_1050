﻿namespace BlApi;
public interface IProduct
{
    /// <summary>
    /// Asking for the list of products
    /// </summary>
    /// <returns>returning ProductForList</returns>
    /// 
   public IEnumerable<BO.ProductForList> GetListOfProducts();
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

    public void UpDateProduct(BO.Product product);



}
