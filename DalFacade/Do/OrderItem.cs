using System.Xml.Linq;

namespace Do;

/// <summary>
/// structure for OrderItem
/// </summary>
public struct OrderItem
{
    /// <summary>
    /// Unique ID number (automatic runner number)
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// Order ID number
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// Product ID number
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Price per unit
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// Amount
    /// </summary>
    public int Amount { get; set; }

    /// <summary>
    /// The function returns a string representing an item in the order
    /// </summary>
    /// <returns>string representing an item in the order</returns>
    public override string ToString() => $@"
        ID: {ID} 
        ProductID: {ProductId},
        OrderId: {OrderId}, 
    	Price: {Price}
    	Amount in stock: {Amount}
        ";
}
