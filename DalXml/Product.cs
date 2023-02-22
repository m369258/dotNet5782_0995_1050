using DalApi;
using Do;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Dal;

internal class Product : IProduct
{

    //[MethodImpl(MethodImplOptions.Synchronized)]
    //public int Add(Do.Product pro)
    //{
    //    XElement? rootConfig = XDocument.Load(@"..\xml\config.xml").Root;
    //    XElement? id = rootConfig?.Element("productId");
    //    int pId = Convert.ToInt32(id?.Value);
    //    pro.ID = pId;
    //    pId++;
    //    id.Value = pId.ToString();
    //    rootConfig?.Save("../xml/config.xml");

    //    XmlRootAttribute xRoot = new XmlRootAttribute();
    //    xRoot.ElementName = "Products";
    //    xRoot.IsNullable = true;

    //    XmlSerializer ser = new XmlSerializer(typeof(List<Product>), xRoot);
    //    StreamReader reader = new StreamReader("..\\xml\\Product.xml");
    //    List<Do.Product> products = (List<Do.Product>)ser.Deserialize(reader);
    //    Do.Product p = products.Where(p => p.ID == pro.ID).FirstOrDefault();
    //    if (p.ID > 0)
    //        throw new Do.DalAlreadyExistsException(p.ID,"this product already exists");
    //    products?.Add(pro);
    //    reader.Close();
    //    StreamWriter writer = new StreamWriter("..\\xml\\Product.xml");
    //    ser.Serialize(writer, products);
    //    writer.Close();
    //    return pro.ID;
    //}
    //[MethodImpl(MethodImplOptions.Synchronized)]
    //public void Delete(int id)
    //{
    //    XmlRootAttribute xRoot = new XmlRootAttribute();
    //    xRoot.ElementName = "Products";
    //    xRoot.IsNullable = true;
    //    XmlSerializer ser = new XmlSerializer(typeof(List<Product>), xRoot);
    //    StreamReader reader = new StreamReader("..\\xml\\Product.xml");
    //    List<Do.Product> products = (List<Do.Product>)ser.Deserialize(reader);
    //    reader.Close();
    //    StreamWriter writer = new StreamWriter("..\\xml\\Product.xml");
    //    Do.Product pro = products.Where(p => p.ID == id).FirstOrDefault();
    //    if (pro.ID == 0)
    //        throw new Do.InvalidInputExseption(pro.ID.ToString());
    //    products.Remove(pro);
    //    ser.Serialize(writer, products);
    //    writer.Close();
    //}
    //[MethodImpl(MethodImplOptions.Synchronized)]
    //public Do.Product Get(Func<Do.Product?, bool> func)
    //{
    //    XmlRootAttribute xRoot = new XmlRootAttribute();
    //    xRoot.ElementName = "Products";
    //    xRoot.IsNullable = true;
    //    XmlSerializer ser = new XmlSerializer(typeof(List<Do.Product>), xRoot);
    //    StreamReader reader = new StreamReader("..\\xml\\Product.xml");
    //    List<Do.Product> products = (List<Do.Product>)ser.Deserialize(reader);
    //    reader.Close();
    //    return products.FirstOrDefault(myProduct => func(myProduct));
    //    // List<Do.Product?> ppp= products.Select(it => (Do.Product?)it).ToList();
    //    // Do.Product? pro = ppp.Where(func).FirstOrDefault();
    //    // if (pro?.ID == 0)
    //    //     throw new Do.InvalidInputExseption(pro?.ID.ToString());
    //    // return ppp.FirstOrDefault(myProduct => func(myProduct)) ??
    //    //throw new Do.DalDoesNotExistException("there are no product with this id");
    //    //return ((Do.Product)(func == null ? products.FirstOrDefault() : pro));
    //}

    //[MethodImpl(MethodImplOptions.Synchronized)]
    //public IEnumerable<Do.Product?> GetAll(Func<Do.Product?, bool>? func = null)
    //{
    //    XmlRootAttribute xRoot = new XmlRootAttribute();
    //    xRoot.ElementName = "Products";
    //    xRoot.IsNullable = true;
    //    XmlSerializer ser = new XmlSerializer(typeof(List<Product>), xRoot);
    //    StreamReader reader = new StreamReader("..\\xml\\Product.xml");
    //    List<Do.Product> products = (List<Do.Product>)ser.Deserialize(reader);
    //    reader.Close();
    //    if (products.Count() == 0)
    //        throw new Do.DalDoesNotExistException("there are no products");
    //    return products.Select(item=>(Do.Product?)item);
    //}


    //[MethodImpl(MethodImplOptions.Synchronized)]
    //public void Update(Do.Product product)
    //{

    //    XmlRootAttribute xRoot = new XmlRootAttribute();
    //    xRoot.ElementName = "Products";
    //    xRoot.IsNullable = true;
    //    XmlSerializer ser = new XmlSerializer(typeof(List<Product>), xRoot);
    //    StreamReader reader = new StreamReader("..\\xml\\Product.xml");
    //    List<Do.Product> products = (List<Do.Product>)ser.Deserialize(reader);
    //    reader.Close();
    //    StreamWriter writer = new StreamWriter("..\\xml\\Product.xml");
    //    Do.Product pro = products.Where(p => p.ID == product.ID).FirstOrDefault();
    //    if (pro.ID == 0)
    //        throw new Do.InvalidInputExseption(pro.ID.ToString());
    //    if (product.InStock < 0)
    //        throw new Do.InvalidInputExseption("invalid amount");

    //    products.Remove(pro);
    //    products.Add(product);
    //    ser.Serialize(writer, products);
    //    writer.Close();
    //}

    //[MethodImpl(MethodImplOptions.Synchronized)]
    //public void updateAmount(int id, int amount)
    //{
    //    if (amount < 0) throw new Do.InvalidInputExseption("invalid amount");
    //    XmlRootAttribute xRoot = new XmlRootAttribute();
    //    xRoot.ElementName = "Products";
    //    xRoot.IsNullable = true;
    //    XmlSerializer ser = new XmlSerializer(typeof(List<Product>), xRoot);
    //    StreamReader reader = new StreamReader("..\\xml\\Product.xml");
    //    List<Do.Product> products = (List<Do.Product>)ser.Deserialize(reader);
    //    reader.Close();
    //    StreamWriter writer = new StreamWriter("..\\xml\\Product.xml");
    //    Do.Product pro = products.Where(p => p.ID == id).FirstOrDefault();
    //    if (pro.ID == 0) throw new Do.InvalidInputExseption(pro.ID.ToString());
    //    Do.Product prod = pro;
    //    prod.InStock = amount;
    //    products.Remove(pro);
    //    products.Add(prod);
    //    ser.Serialize(writer, products);
    //    writer.Close();
    //}

    const string s_product = @"Product";
    public int Add(Do.Product entity)
    {
        List<Do.Product?> listProducts = Tools.LoadListFromXMLSerializer<Do.Product>(s_product);
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

    public void Delete(int id)
    {
        List<Do.Product?> listProducts = Tools.LoadListFromXMLSerializer<Do.Product>(s_product);

        if (listProducts.RemoveAll(order => order?.ID == id) == 0)
            throw new Do.DalDoesNotExistException(id, "product", "there is no this id product");

        Tools.SaveListToXMLSerializer(listProducts, s_product);
    }

    public Do.Product Get(Func<Do.Product?, bool> condition)
    {
        List<Do.Product?> listProducts = Tools.LoadListFromXMLSerializer<Do.Product>(s_product);
        return listProducts.FirstOrDefault(myProduct => condition(myProduct)) ??
      throw new Do.DalDoesNotExistException("there are no product with this id");
    }

    public IEnumerable<Do.Product?> GetAll(Func<Do.Product?, bool>? condition = null)
    {
        List<Do.Product?> listProducts = Tools.LoadListFromXMLSerializer<Do.Product>(s_product);
        return condition != null ?
              listProducts.Where(currProduct => condition(currProduct)) :
              listProducts.Select(currProduct => currProduct);
    }

    public void Update(Do.Product updateEntity)
    {
        Delete(updateEntity.ID);
        Add(updateEntity);
    }
}
