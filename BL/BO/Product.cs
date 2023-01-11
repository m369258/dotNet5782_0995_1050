using System.Diagnostics;
using System.Xml.Linq;

namespace BO;

public class Product
{
    /// <summary>
    /// Unique ID number (like the barcode number of the product)
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// Product Name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// category
    /// </summary>
    public Category? Category { get; set; }

    /// <summary>
    /// price
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// Units in Stock
    /// </summary>
    public int InStock { get; set; }

    public string? Img { get; set; }

    public override string ToString() => this.ToStringProperty();
}
