using Do;
namespace DalApi;

public interface IOrderItems:ICrud<OrderItem>
{
    /// <summary>
    /// This action gets an order ID number and returns all the order details
    /// </summary>
    /// <param name="idOrder">Order ID number</param>
    /// <returns>All order details</returns>
    public IEnumerable<OrderItem> GetByIdOrder(int idOrder);

    /// <summary>
    /// This operation receives an order and product ID number and returns a requested order item
    /// </summary>
    /// <param name="idOrder">Order ID number</param>
    /// <param name="idProduct">Product ID number.</param>
    /// <returns> a requested order item</returns>
    public OrderItem Get(int idOrder, int idProduct);


}
