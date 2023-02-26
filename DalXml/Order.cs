using DalApi;
using Do;
using System.Runtime.CompilerServices;

namespace Dal;

internal class Order : IOrder
{
    const string s_order = @"Order";

    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Add(Do.Order entity)
    {
        List<Do.Order?> listOrders = Tools.LoadListFromXMLSerializer<Do.Order>(s_order);

        if (listOrders.FirstOrDefault(order => order?.ID == entity.ID) != null)
            throw new Exception("id aleardy exsist");

        entity.ID = Tools.getNextID(@"NextOrderId");
        listOrders.Add(entity);
        Tools.SaveListToXMLSerializer(listOrders,s_order);
        return entity.ID;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
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

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Do.Order?> GetAll(Func<Do.Order?, bool>? condition = null)
    {
        List<Do.Order?> listOrders = Tools.LoadListFromXMLSerializer<Do.Order>(s_order);
        if (condition == null)
            return listOrders.Select(ls=>ls);
        else
            return listOrders.Where(condition);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(Do.Order updateEntity)
    {
        List<Do.Order?> listOrders = Tools.LoadListFromXMLSerializer<Do.Order>(s_order);

        if (listOrders.RemoveAll(order => order?.ID == updateEntity.ID) == 0)
            throw new Exception("Missing id");
        listOrders.Add(updateEntity);

        Tools.SaveListToXMLSerializer(listOrders, s_order);

    }
}
