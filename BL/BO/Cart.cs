namespace BO;

/// <summary>
/// For the shopping basket management and order confirmation screen, which will contain the following.
/// </summary>
public class Cart
{
    /// <summary>
    /// buyer name
    /// </summary>
    public string CustomerName { get; set; }

    /// <summary>
    /// Buyer's email address
    /// </summary>
    public string CustomerEmail { get; set; }

    /// <summary>
    /// Buyer's address
    /// </summary>
    public string CustomerAddress { get; set; }

    /// <summary>
    /// List of order details
    /// </summary>
    public List<OrderItem> items { get; set; }

    /// <summary>
    /// Total price of an order basket
    /// </summary>
    public double TotalPrice { get; set; }
}
