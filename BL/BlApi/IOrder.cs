namespace BlApi;

public interface IOrder
{
    public IEnumerable<BO.OrderForList> GetListOfOrders();
    public BO.Order OrderShippingUpdate(int orderId);

    public BO.Order GetOrderDetails(int idOrder);

    public BO.Order OrderDeliveryUpdate(int idOrder);

}
