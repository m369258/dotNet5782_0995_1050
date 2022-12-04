using Do;
namespace Dal;
using DalApi;

internal class DalProduct : IProduct
{

    /// <summary>
    /// This action adds an product to the system if there is an available space
    /// </summary>
    /// <param name="order">Product to add</param>
    /// <returns>Return the ID number of the added object</returns>
    /// <exception cref="Exception">If there is no space available for a new order, an error will be thrown</exception>
    public int Add(Product product)
    {
        int i = 0;
        //The loop checks if there is a product with the requested ID number, if so it will throw an error
        for (i = 0; i < DataSource.products.Count
        &&
        DataSource.products[i].ID != product.ID;
        i++) ;
        if (i != DataSource.products.Count)
        {
            throw new Do.DalAlreadyExistsException(product.ID,"product","this product is exsist");
        }
        DataSource.products.Add(product);
        return product.ID;
    }


    /// <summary>
    /// This returns the correct product by some ID number
    /// </summary>
    /// <param name="idProduct">ID number of desired product</param>
    /// <returns>Returns the desired product</returns>
    /// <exception cref="Exception">If the required product does not exist, an error will be thrown</exception>
    public Product Get(int idProduct)
    {
        int i = 0;
        //The loop searches for the location of the product
        while (i < DataSource.products.Count && DataSource.products[i].ID != idProduct)
        {
            i++;
        }
        //Checking whether the requested product is found and returning it otherwise throws an error
        if (DataSource.products[i].ID == idProduct)
            return DataSource.products[i];
        throw new Do.DalDoesNotExistException(idProduct,"product","there are no product with this id");
    }


    /// <summary>
    /// This returns all products
    /// </summary>
    /// <returns>All products</returns>
    public IEnumerable<Product> GetAll()
    {
        List<Product> newProduct = new List<Product>();
        //The loop performs the explicit copying of the array of products
        for (int i = 0; i < DataSource.products.Count; i++)
        {
            newProduct.Add(DataSource.products[i]);
        }
        return newProduct;
    }

    /// <summary>
    /// This operation gets an product ID number and deletes it if it exists, otherwise an error will be thrown
    /// </summary>
    /// <param name="idProduct">Product ID number</param>
    /// <exception cref="Exception">In case the product does not exist in the database, an error will be thrown</exception>
    public void Delete(int idProduct)
    {
        int ind = GetIndex(idProduct);
        if (ind != -1)
        {
            DataSource.products.RemoveAt(ind);
        }
        else
            throw new Do.DalDoesNotExistException(idProduct,"product","there is no this id product");
    }

    /// <summary>
    /// This function receives an ID number of an product and returns its position in the array
    /// </summary>
    /// <param name="idProduct">Product ID number</param>
    /// <returns>its position in the array of the requested product</returns>
    private int GetIndex(int idProduct)
    {
        int i = 0;
        //The loop searches for the location of the requested product
        while (i < DataSource.products.Count && DataSource.products[i].ID != idProduct)
        {
            i++;
        }
        //If the order is found, its position in the product array will be returned, otherwise -1 will be returned
        if (DataSource.products[i].ID == idProduct)
            return i;
        return -1;
    }


    /// <summary>
    /// This operation accepts an product and updates its details if it exists, otherwise it will throw an error
    /// </summary>
    /// <param name="updateProduct">Product to update</param>
    /// <exception cref="Exception">Throw an error if the requested product does not exist</exception>
    public void Update(Product updateProduct)
    {
        int ind = GetIndex(updateProduct.ID);
        if (ind != -1)
        {
            DataSource.products[ind] = updateProduct;
        }
        else { throw new Do.DalDoesNotExistException(updateProduct.ID,"product","there is no product like this"); }
    }

}

