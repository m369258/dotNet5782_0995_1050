using DalApi;
using Do;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Dal;

internal class OrderItem : IOrderItems
{
    //[MethodImpl(MethodImplOptions.Synchronized)]
    //public int Add(Do.OrderItem oi)
    //{
    //    XElement? rootConfig = XDocument.Load(@"..\xml\config.xml").Root;
    //    XElement? id = rootConfig?.Element("orderItemId");
    //    int oiId = Convert.ToInt32(id?.Value);
    //    oi.ID = oiId;
    //    oiId++;
    //    id.Value = oiId.ToString();
    //    rootConfig?.Save("../xml/config.xml");
    //    XmlRootAttribute xRoot = new XmlRootAttribute();
    //    xRoot.ElementName = "orderItems";
    //    xRoot.IsNullable = true;
    //    XmlSerializer ser = new XmlSerializer(typeof(List<OrderItem>), xRoot);
    //    StreamReader reader = new StreamReader("..\\xml\\OrderItem.xml");
    //    List<Do.OrderItem> orderItems = (List<Do.OrderItem>)ser.Deserialize(reader);
    //    reader.Close();
    //    orderItems?.Add(oi);
    //    StreamWriter writer = new StreamWriter("..\\xml\\OrderItem.xml");
    //    ser.Serialize(writer, orderItems);
    //    writer.Close();
    //    return oi.ID;
    //}
    //[MethodImpl(MethodImplOptions.Synchronized)]
    //public void Delete(int id)
    //{
    //    XmlRootAttribute xRoot = new XmlRootAttribute();
    //    xRoot.ElementName = "orderItems";
    //    xRoot.IsNullable = true;
    //    XmlSerializer ser = new XmlSerializer(typeof(List<OrderItem>), xRoot);
    //    StreamReader reader = new StreamReader("..\\xml\\OrderItem.xml");
    //    List<Do.OrderItem> orderItems = (List<Do.OrderItem>)ser.Deserialize(reader);
    //    reader.Close();
    //    StreamWriter writer = new StreamWriter("..\\xml\\OrderItem.xml");
    //    Do.OrderItem oi = orderItems.Where(p => p.ID == id).FirstOrDefault();
    //    orderItems.Remove(oi);
    //    ser.Serialize(writer, orderItems);
    //    writer.Close();
    //}
    //[MethodImpl(MethodImplOptions.Synchronized)]
    //public Do.OrderItem Get(Func<Do.OrderItem?, bool>? func)
    //{
    //    XmlRootAttribute xRoot = new XmlRootAttribute();
    //    xRoot.ElementName = "orderItems";
    //    xRoot.IsNullable = true;
    //    XmlSerializer ser = new XmlSerializer(typeof(List<Do.OrderItem>), xRoot);
    //    StreamReader reader = new StreamReader("..\\xml\\OrderItem.xml");
    //    List<Do.OrderItem?> ois = (List<Do.OrderItem?>)ser.Deserialize(reader);
    //    reader.Close();
    //    return (Do.OrderItem)(func == null ? ois : ois.Where(func).ToList()).FirstOrDefault();
    //}
    //[MethodImpl(MethodImplOptions.Synchronized)]
    //public IEnumerable<Do.OrderItem?> GetAll(Func<Do.OrderItem?, bool>? func = null)
    //{
    //    XmlRootAttribute xRoot = new XmlRootAttribute();
    //    xRoot.ElementName = "orderItems";
    //    xRoot.IsNullable = true;
    //    XmlSerializer ser = new XmlSerializer(typeof(List<Do.OrderItem>), xRoot);
    //    StreamReader reader = new StreamReader("..\\xml\\OrderItem.xml");
    //    List<Do.OrderItem> ois = (List<Do.OrderItem>)ser.Deserialize(reader);
    //    reader.Close();
    //    //return ois.Select(x=>(Do.OrderItem?)x);

    //    IEnumerable<Do.OrderItem> temp= func != null ?
    //          ois.Where(currOrderItem => func(currOrderItem)) :
    //          ois.Select(currOrderItem => currOrderItem);
    //    return temp.Select(item => (Do.OrderItem?)item);
    //}
    ////[MethodImpl(MethodImplOptions.Synchronized)]
    ////public IEnumerable<Do.OrderItem> getByOrderId(int orderId)
    ////{
    ////    XmlRootAttribute xRoot = new XmlRootAttribute();
    ////    xRoot.ElementName = "orderItems";
    ////    xRoot.IsNullable = true;
    ////    XmlSerializer ser = new XmlSerializer(typeof(List<OrderItem>), xRoot);
    ////    StreamReader reader = new StreamReader("..\\xml\\OrderItem.xml");
    ////    List<Do.OrderItem> ois = (List<Do.OrderItem>)ser.Deserialize(reader);
    ////    reader.Close();
    ////    return ois.Where(oit => oit.OrderId == orderId).ToList();
    ////}
    //[MethodImpl(MethodImplOptions.Synchronized)]
    //public void Update(Do.OrderItem oi)
    //{
    //    XmlRootAttribute xRoot = new XmlRootAttribute();
    //    xRoot.ElementName = "orderItems";
    //    xRoot.IsNullable = true;
    //    XmlSerializer ser = new XmlSerializer(typeof(List<OrderItem>), xRoot);
    //    StreamReader reader = new StreamReader("..\\xml\\OrderItem.xml");
    //    List<Do.OrderItem> ois = (List<Do.OrderItem>)ser.Deserialize(reader);
    //    reader.Close();
    //    StreamWriter writer = new StreamWriter("..\\xml\\OrderItem.xml");
    //    Do.OrderItem orderItem = ois.Where(p => p.ID == oi.ID).FirstOrDefault();
    //    ois.Remove(orderItem);
    //    ois.Add(oi);
    //    ser.Serialize(writer, ois);
    //    writer.Close();
    //}
    const string s_orderItem = @"OrderItem";
    const string s_product = @"Product";
    const string s_order = @"Order";

    public int Add(Do.OrderItem orderItem)
    {
        List<Do.OrderItem?> listOrderItems = Tools.LoadListFromXMLSerializer<Do.OrderItem>(s_orderItem);
        List<Do.Product?> listProducts = Tools.LoadListFromXMLSerializer<Do.Product>(s_product);
        List<Do.Order?> listOrders = Tools.LoadListFromXMLSerializer<Do.Order>(s_order);


        var ids = Tools.LoadListFromXMLSerializer<int>(@"config");
        // return (Tools.LoadListFromXMLSerializer(s_orderItem), "NextOrderItem") ?? throw new NullReferenceException();

        orderItem.ID = 1;/*(ids.Select(x=>x== NextOrderItem));*/
        int i;
        //Checking whether the product ID exists in any other case will throw an error
        Do.Product? p = listProducts.Find(currenProduct => { return (currenProduct?.ID == orderItem.ProductId); });

        for (i = 0; i < listProducts.Count && listProducts[i]?.ID != orderItem.ProductId; i++) ;
        if (i == listProducts.Count)
        {
            throw new Do.DalAlreadyExistsException(orderItem.ID, "orderItem", "this product is exsist");
        }

        //Checking if the order ID exists in any other case will throw an error
        for (i = 0; i < listOrders.Count && listOrders[i]?.ID != orderItem.OrderId; i++) ;
        if (i == listOrders.Count)
        {
            throw new Do.DalAlreadyExistsException(orderItem.ID, "orderItem", "this order is exsist");
        }

        //Adding the order item to the database and updating the actual quantity
        listOrderItems.Add(orderItem);
        Tools.SaveListToXMLSerializer(listOrderItems, s_orderItem);


        return orderItem.ID;
    }

    public void Delete(int id)
    {
        List<Do.OrderItem?> listOrderItem = Tools.LoadListFromXMLSerializer<Do.OrderItem>(s_orderItem);

        if (listOrderItem.RemoveAll(order => order?.ID == id) == 0)
            throw new Do.DalDoesNotExistException(id, "orderItem", "there is no this id orderItem");

        Tools.SaveListToXMLSerializer(listOrderItem, s_orderItem);
    }

    public Do.OrderItem Get(Func<Do.OrderItem?, bool> condition)
    {
        List<Do.OrderItem?> listOrderItem = Tools.LoadListFromXMLSerializer<Do.OrderItem>(s_orderItem);
        return listOrderItem.FirstOrDefault(myOrderItem => condition(myOrderItem)) ??
        throw new Do.DalDoesNotExistException("there are no orderItem with this id");
    }

    public IEnumerable<Do.OrderItem?> GetAll(Func<Do.OrderItem?, bool>? condition = null)
    {
        List<Do.OrderItem?> listOrderItem = Tools.LoadListFromXMLSerializer<Do.OrderItem>(s_orderItem);
        if (condition == null)
            return listOrderItem;
        else
            return listOrderItem.Where(condition);
    }

    public void Update(Do.OrderItem updateEntity)
    {
        Delete(updateEntity.ID);
        Add(updateEntity);
    }
}
