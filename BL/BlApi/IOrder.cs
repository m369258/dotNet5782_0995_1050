namespace BlApi;

public interface IOrder
{
    /// <summary>
    /// Order list request
    /// </summary>
    /// <returns>Order list</returns>
    public IEnumerable<BO.OrderForList> GetListOfOrders();

    /// <summary>
    /// Order shipping update
    /// </summary>
    /// <param name="orderId">the order id to update</param>
    /// <returns>the update order</returns>
    public BO.Order OrderShippingUpdate(int orderId);

    public BO.Order GetOrderDetails(int idOrder);

    public BO.Order OrderDeliveryUpdate(int idOrder);

    public BO.OrderTracking OrderTracking(int idOrder);

}
