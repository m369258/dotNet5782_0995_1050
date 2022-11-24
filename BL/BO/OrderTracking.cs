
namespace BO;

/// <summary>
/// For an order tracking screen, which will contain the following.
/// </summary>
public class OrderTracking
{
    /// <summary>
    /// ID (order)
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// Order status
    /// </summary>
    public OrderStatus Status { get; set; }

    /// <summary>
    /// (רשימה של צמדים (תאריך, תיאור התקדמות חבילה
    /// </summary>
    public IEnumerable<Tuple<DateTime, string>> Tracking { get; set; }
}
