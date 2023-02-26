using DalApi;
using System.Runtime.CompilerServices;

namespace Dal;

internal class Product : IProduct
{
    const string s_product = @"Product";

    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Add(Do.Product entity)
    {
        List<Do.Product?> listProducts = Tools.LoadListFromXMLSerializer<Do.Product>(s_product);
        //var ids = Tools.LoadListFromXMLSerializer<int>(s_config);
        int i = 0;
        //The loop checks if there is a product with the requested ID number, if so it will throw an error
        for (i = 0; i < listProducts.Count
        &&
        listProducts[i]?.ID != entity.ID;
        i++) ;
        if (i != listProducts.Count)
        {
            throw new Do.DalAlreadyExistsException(entity.ID, "product", "this product is exsist");
        }
        listProducts.Add(entity);
        Tools.SaveListToXMLSerializer(listProducts, s_product);
        return entity.ID;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int id)
    {
        List<Do.Product?> listProducts = Tools.LoadListFromXMLSerializer<Do.Product>(s_product);

        if (listProducts.RemoveAll(order => order?.ID == id) == 0)
            throw new Do.DalDoesNotExistException(id, "product", "there is no this id product");

        Tools.SaveListToXMLSerializer(listProducts, s_product);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public Do.Product Get(Func<Do.Product?, bool> condition)
    {
        List<Do.Product?> listProducts = Tools.LoadListFromXMLSerializer<Do.Product>(s_product);
        return listProducts.FirstOrDefault(myProduct => condition(myProduct)) ??
      throw new Do.DalDoesNotExistException("there are no product with this id");
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Do.Product?> GetAll(Func<Do.Product?, bool>? condition = null)
    {
        List<Do.Product?> listProducts = Tools.LoadListFromXMLSerializer<Do.Product>(s_product);
        return condition != null ?
              listProducts.Where(currProduct => condition(currProduct)) :
              listProducts.Select(currProduct => currProduct);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(Do.Product updateEntity)
    {
        Delete(updateEntity.ID);
        Add(updateEntity);
    }
}
