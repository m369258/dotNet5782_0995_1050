namespace BlApi;

public interface IOrder
{
    public IEnumerable<BO.OrderForList> GetListOfOrders();
    public BO.Order OrderShippingUpdate(int orderId);

}
