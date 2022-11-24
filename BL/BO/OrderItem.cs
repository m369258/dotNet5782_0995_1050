using System.Runtime.ConstrainedExecution;

namespace BO;

internal class OrderItem
{
    /// <summary>
    /// Unique ID number (automatic runner number)
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// Product ID number
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Product name
    /// </summary>
    public string NameProduct { get; set; }


    /// <summary>
    /// Price per unit
    /// </summary>
    public double productPrice { get; set; }

    /// <summary>
    /// Quantity of items of a product in the basket
    /// </summary>
    public int QuantityPerItem { get; set; }

    /// <summary>
    /// Quantity per item
    /// </summary>
    public double TotalPrice { get; set; }
}
