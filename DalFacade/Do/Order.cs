using System.Diagnostics;
using System.Xml.Linq;

namespace Do;


/// <summary>
/// structure of order
/// </summary>
public struct Order
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
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// delivery date
    /// </summary>
    public DateTime DeliveryDate { get; set; }

    /// <summary>
    /// Date of delivery
    /// </summary>
    public DateTime ShipDate { get; set; }

    /// <summary>
    /// The function returns a string representing a order
    /// </summary>
    /// <returns>a string representing a order</returns>
    public override string ToString() => 
        $@"
    Order ID: {ID}
    CustomerName: {CustomerName}, 
    CustomerEmail: {CustomerEmail}
    CustomerAddress: {CustomerAddress}
    OrderDate: {OrderDate}
    ShipDate: {ShipDate}
    DeliveryDate: {DeliveryDate}
    ";
}
