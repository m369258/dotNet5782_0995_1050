namespace BO;

public class Order
{
    /// <summary>
    /// Unique ID number (automatic runner number)
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// The name of the ordering customer
    /// </summary>
    public string CustomerName { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string CustomerEmail { get; set; }

    /// <summary>
    /// shipping address
    /// </summary>
    public string CustomerAddress { get; set; }

    /// <summary>
    /// Order creation date
    /// </summary>
    public DateTime PaymentDate { get; set; }

    /// <summary>
    /// Order status
    /// </summary>
    public OrderStatus status { get; set; }

    /// <summary>
    /// delivery date
    /// </summary>
    public DateTime DeliveryDate { get; set; }

    /// <summary>
    /// Date of delivery
    /// </summary>
    public DateTime ShipDate { get; set; }

    /// <summary>
    /// List of order details
    /// </summary>
    public List<OrderItem> items { get; set; }

    /// <summary>
    /// Total price of order
    /// </summary>
    public double totalPrice { get; set; }
}
