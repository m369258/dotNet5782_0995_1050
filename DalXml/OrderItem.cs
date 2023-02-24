using DalApi;
namespace Dal;

internal class OrderItem : IOrderItems
{
    const string s_orderItem = @"OrderItem";
    const string s_product = @"Product";
    const string s_order = @"Order";

    public int Add(Do.OrderItem orderItem)
    {
        List<Do.OrderItem?> listOrderItems = Tools.LoadListFromXMLSerializer<Do.OrderItem>(s_orderItem);
        List<Do.Product?> listProducts = Tools.LoadListFromXMLSerializer<Do.Product>(s_product);
        List<Do.Order?> listOrders = Tools.LoadListFromXMLSerializer<Do.Order>(s_order);

        orderItem.ID = Tools.getNextID(@"NextOrderItem");
        
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
        //Delete(updateEntity.ID);
        //Add(updateEntity);

        List<Do.OrderItem?> listOrdeItemss = Tools.LoadListFromXMLSerializer<Do.OrderItem>(s_orderItem);

        if (listOrdeItemss.RemoveAll(item => item?.ID == updateEntity.ID) == 0)
            throw new Exception("Missing id");
        listOrdeItemss.Add(updateEntity);

        Tools.SaveListToXMLSerializer(listOrdeItemss, s_orderItem);
    }
}
