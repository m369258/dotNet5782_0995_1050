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

    /// <summary>
    /// Order details request
    /// </summary>
    /// <param name="idOrder">id of order</param>
    /// <returns>will return a constructed order object</returns>
    public BO.Order GetOrderDetails(int idOrder);

    /// <summary>
    /// Order delivery update
    /// </summary>
    /// <param name="idOrder">id of order</param>
    /// <returns>Pig order object (logical entity) updated</returns>
    public BO.Order OrderDeliveryUpdate(int idOrder);

    /// <summary>
    /// Order Tracking
    /// </summary>
    /// <param name="idOrder">id of order</param>
    /// <returns>will return an instance of OrderTracking (logical entity) appended to the order ID and its current state
    /// , will contain a list of date pairs and a verbal description of the state</returns>
    public BO.OrderTracking OrderTracking(int idOrder);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public IEnumerable<BO.OrderTracking?> OrdersOfUsers(string? email=null);

}
