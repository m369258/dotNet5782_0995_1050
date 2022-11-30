namespace BO;

/// <summary>
/// A helper entity of a list order
/// </summary>
public class OrderForList
{
    /// <summary>
    /// Order ID
    /// </summary>
    public int OrderID{ get; set; }
    /// <summary>
    /// Buyer name
    /// </summary>
    public string CustomerName{ get; set; }

    /// <summary>
    /// Order status
    /// </summary>
    public OrderStatus status { get; set; }

    /// <summary>
    /// Amount of items
    /// </summary>
    public int AmountForItems { get; set; }

    /// <summary>
    /// Total price
    /// </summary>
    public double TotalPrice { get; set; }

    public override string ToString() => $@"
         OrderID: {OrderID}
        CustomerName:{CustomerName}
        status: {status}, 
        AmountForItems: {AmountForItems}
    	TotalPrice: {TotalPrice}
        ";

}
