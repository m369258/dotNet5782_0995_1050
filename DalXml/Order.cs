using DO;
using DalApi;
using Do;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Dal;

internal class Order : IOrder
{

    //[MethodImpl(MethodImplOptions.Synchronized)]
    //public int Add(Do.Order order)
    //{
    //    XElement? rootConfig = XDocument.Load(@"..\xml\config.xml").Root;
    //    //XElement? id = rootConfig?.Element("ID");
    //    XElement? id = rootConfig?.Element("orderId");
    //    int oId = Convert.ToInt32(id?.Value);
    //   order.ID= oId;
    //    oId++;
    //    id.Value = oId.ToString();
    //    rootConfig?.Save("../xml/config.xml");
    //    XElement? orderElement = XDocument.Load(@"../xml/Order.xml").Root;
    //    XElement? order1 = new XElement("order",
    //    new XElement("ID", order.ID),
    //    new XElement("CustomerName", order.CustomerName),
    //    new XElement("CustomerAddress", order.CustomerAddress),
    //    new XElement("CustomerEmail", order.CustomerEmail),
    //    new XElement("ShipDate", order.ShipDate?.ToShortDateString()),
    //    new XElement("DeliveryDate", order.DeliveryDate?.ToShortDateString()),
    //    new XElement("OrderDate", order.OrderDate?.ToShortDateString()));
    //    orderElement?.Add(order1);
    //    orderElement?.Save(@"../xml/Order.xml");
    //    return order.ID;
    //}
    //[MethodImpl(MethodImplOptions.Synchronized)]
    //public void Delete(int id)
    //{
    //    XElement? root = XDocument.Load("../xml/Order.xml").Root;
    //    root?.Descendants("order").Where(p => int.Parse(p?.Element("ID").Value) == id).Remove();
    //    root?.Save("../xml/Order.xml");
    //}
    //[MethodImpl(MethodImplOptions.Synchronized)]
    //public Do.Order deepCopy(XElement? o)
    //{
    //    Do.Order order = new Do.Order();
    //    order.ID = Convert.ToInt32(o?.Element("ID")?.Value);
    //    order.CustomerName = o?.Element("CustomerName")?.Value;
    //    order.CustomerEmail = o?.Element("CustomerEmail")?.Value;
    //    order.CustomerAddress = o?.Element("CustomerAddress")?.Value;
    //    order.OrderDate = Convert.ToDateTime(o?.Element("OrderDate")?.Value);
    //    order.ShipDate = o?.Element("ShipDate")?.Value != "" ? Convert.ToDateTime(o?.Element("ShipDate")?.Value) : null;
    //    order.DeliveryDate = o?.Element("DeliveryDate")?.Value != "" ? Convert.ToDateTime(o?.Element("DeliveryDate")?.Value) : null;
    //    return order;
    //}
    //[MethodImpl(MethodImplOptions.Synchronized)]
    //public Do.Order Get(Func<Do.Order?, bool> func)
    //{
    //    IEnumerable<Do.Order?> orders = GetAll();
    //    return (Do.Order)(func == null ? orders : orders.Where(func).ToList()).FirstOrDefault();
    //}
    //[MethodImpl(MethodImplOptions.Synchronized)]
    //public IEnumerable<Do.Order?> GetAll(Func<Do.Order?, bool>? func=null)
    //{
    //    XElement? root = XDocument.Load("../xml/Order.xml")?.Root;
    //    IEnumerable<XElement>? orderList = root?.Descendants("order")?.ToList();
    //    List<Do.Order?> orders = new List<Do.Order?>();
    //    orderList.Select(item =>
    //    {
    //        orders.Add(deepCopy(item));

    //        return item;
    //    }).ToList();

    //    return (func == null ? orders : orders.Where(func).ToList());
    //    throw new NotImplementedException();
    //}
    //[MethodImpl(MethodImplOptions.Synchronized)]
    //public void Update(Do.Order ord)
    //{
    //    XElement? root = XDocument.Load("../xml/Order.xml").Root;
    //    XElement? order = root?.Elements("order")?.Where(o => o.Element("ID")?.Value == ord.ID.ToString()).FirstOrDefault();
    //    if (order == null)
    //        throw new NotImplementedException();
    //    XElement o = new("order",
    //                    new XElement("ID", ord.ID),
    //                    new XElement("CustomerName", ord.CustomerName),
    //                    new XElement("CustomerEmail", ord.CustomerEmail),
    //                    new XElement("CustomerAddress", ord.CustomerAddress),
    //                    new XElement("OrderDate", ord.OrderDate),
    //                    new XElement("ShipDate", ord.ShipDate),
    //                    new XElement("DeliveryDate", ord.DeliveryDate));
    //    order.Remove();
    //    root?.Add(o);
    //    root?.Save("../xml/Order.xml");
    //}
    const string s_order = @"Order";
    public int Add(Do.Order entity)
    {
        List<Do.Order?> listOrders = Tools.LoadListFromXMLSerializer<Do.Order>(s_order);

        if (listOrders.FirstOrDefault(order => order?.ID == entity.ID) != null)
            throw new Exception("id aleardy exsist");
        entity.ID = 44;
        listOrders.Add(entity);
        Tools.SaveListToXMLSerializer(listOrders,s_order);
        return entity.ID;
    }

    public void Delete(int id)
    {
        List<Do.Order?> listOrders = Tools.LoadListFromXMLSerializer<Do.Order>(s_order);

        if (listOrders.RemoveAll(order => order?.ID == id) == 0)
            throw new Exception("Missing id");

        Tools.SaveListToXMLSerializer(listOrders, s_order);
    }

    public Do.Order Get(Func<Do.Order?, bool> condition)
    {
        List<Do.Order?> listOrders = Tools.LoadListFromXMLSerializer<Do.Order>(s_order);
        return listOrders.FirstOrDefault(myOrder => condition(myOrder)) ??
            throw new Do.DalDoesNotExistException("there are no order with this id");
    }

    public IEnumerable<Do.Order?> GetAll(Func<Do.Order?, bool>? condition = null)
    {
        List<Do.Order?> listOrders = Tools.LoadListFromXMLSerializer<Do.Order>(s_order);
        if (condition == null)
            return listOrders.Select(ls=>ls);
        else
            return listOrders.Where(condition);
    }

    public void Update(Do.Order updateEntity)
    {
        Delete(updateEntity.ID);
        Add(updateEntity);
    }
}
