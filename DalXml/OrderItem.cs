using DalApi;
using Do;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Dal;

internal class OrderItem : IOrderItems
{
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Add(Do.OrderItem oi)
    {
        XElement? rootConfig = XDocument.Load(@"..\xml\config.xml").Root;
        XElement? id = rootConfig?.Element("orderItemId");
        int oiId = Convert.ToInt32(id?.Value);
        oi.ID = oiId;
        oiId++;
        id.Value = oiId.ToString();
        rootConfig?.Save("../xml/config.xml");
        XmlRootAttribute xRoot = new XmlRootAttribute();
        xRoot.ElementName = "orderItems";
        xRoot.IsNullable = true;
        XmlSerializer ser = new XmlSerializer(typeof(List<OrderItem>), xRoot);
        StreamReader reader = new StreamReader("..\\xml\\OrderItem.xml");
        List<Do.OrderItem> orderItems = (List<Do.OrderItem>)ser.Deserialize(reader);
        reader.Close();
        orderItems?.Add(oi);
        StreamWriter writer = new StreamWriter("..\\xml\\OrderItem.xml");
        ser.Serialize(writer, orderItems);
        writer.Close();
        return oi.ID;
    }
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int id)
    {
        XmlRootAttribute xRoot = new XmlRootAttribute();
        xRoot.ElementName = "orderItems";
        xRoot.IsNullable = true;
        XmlSerializer ser = new XmlSerializer(typeof(List<OrderItem>), xRoot);
        StreamReader reader = new StreamReader("..\\xml\\OrderItem.xml");
        List<Do.OrderItem> orderItems = (List<Do.OrderItem>)ser.Deserialize(reader);
        reader.Close();
        StreamWriter writer = new StreamWriter("..\\xml\\OrderItem.xml");
        Do.OrderItem oi = orderItems.Where(p => p.ID == id).FirstOrDefault();
        orderItems.Remove(oi);
        ser.Serialize(writer, orderItems);
        writer.Close();
    }
    [MethodImpl(MethodImplOptions.Synchronized)]
    public Do.OrderItem Get(Func<Do.OrderItem?, bool>? func)
    {
        XmlRootAttribute xRoot = new XmlRootAttribute();
        xRoot.ElementName = "orderItems";
        xRoot.IsNullable = true;
        XmlSerializer ser = new XmlSerializer(typeof(List<Do.OrderItem>), xRoot);
        StreamReader reader = new StreamReader("..\\xml\\OrderItem.xml");
        List<Do.OrderItem?> ois = (List<Do.OrderItem?>)ser.Deserialize(reader);
        reader.Close();
        return (Do.OrderItem)(func == null ? ois : ois.Where(func).ToList()).FirstOrDefault();
    }
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Do.OrderItem?> GetAll(Func<Do.OrderItem?, bool>? func = null)
    {
        XmlRootAttribute xRoot = new XmlRootAttribute();
        xRoot.ElementName = "orderItems";
        xRoot.IsNullable = true;
        XmlSerializer ser = new XmlSerializer(typeof(List<Do.OrderItem>), xRoot);
        StreamReader reader = new StreamReader("..\\xml\\OrderItem.xml");
        List<Do.OrderItem> ois = (List<Do.OrderItem>)ser.Deserialize(reader);
        reader.Close();
        //return ois.Select(x=>(Do.OrderItem?)x);

        IEnumerable<Do.OrderItem> temp= func != null ?
              ois.Where(currOrderItem => func(currOrderItem)) :
              ois.Select(currOrderItem => currOrderItem);
        return temp.Select(item => (Do.OrderItem?)item);
    }
    //[MethodImpl(MethodImplOptions.Synchronized)]
    //public IEnumerable<Do.OrderItem> getByOrderId(int orderId)
    //{
    //    XmlRootAttribute xRoot = new XmlRootAttribute();
    //    xRoot.ElementName = "orderItems";
    //    xRoot.IsNullable = true;
    //    XmlSerializer ser = new XmlSerializer(typeof(List<OrderItem>), xRoot);
    //    StreamReader reader = new StreamReader("..\\xml\\OrderItem.xml");
    //    List<Do.OrderItem> ois = (List<Do.OrderItem>)ser.Deserialize(reader);
    //    reader.Close();
    //    return ois.Where(oit => oit.OrderId == orderId).ToList();
    //}
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(Do.OrderItem oi)
    {
        XmlRootAttribute xRoot = new XmlRootAttribute();
        xRoot.ElementName = "orderItems";
        xRoot.IsNullable = true;
        XmlSerializer ser = new XmlSerializer(typeof(List<OrderItem>), xRoot);
        StreamReader reader = new StreamReader("..\\xml\\OrderItem.xml");
        List<Do.OrderItem> ois = (List<Do.OrderItem>)ser.Deserialize(reader);
        reader.Close();
        StreamWriter writer = new StreamWriter("..\\xml\\OrderItem.xml");
        Do.OrderItem orderItem = ois.Where(p => p.ID == oi.ID).FirstOrDefault();
        ois.Remove(orderItem);
        ois.Add(oi);
        ser.Serialize(writer, ois);
        writer.Close();
    }
}
