using DO;
namespace Do;

/// <summary>
/// structure for product
/// </summary>
public struct Product
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

    /// <summary>
    /// The function returns a string representing a product
    /// </summary>
    /// <returns>a string representing a product</returns>
    public override string ToString() => this.ToStringProperty();
}
