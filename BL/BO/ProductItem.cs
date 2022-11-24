namespace BO;

/// <summary>
/// Auxiliary entity of a product item (representing a product for the catalog) ProductItem - for the catalog screen - with the list of products shown to the buyer, which will contain:
/// </summary>
public class ProductItem
{
    /// <summary>
    /// Product ID
    /// </summary>
    public int ProductID { get; set; }

    /// <summary>
    /// Product name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Product price
    /// </summary>
    public double Price { get; set; }
    /// <summary>
    /// Product category
    /// </summary>
    public Category Category { get; set; }

    /// <summary>
    /// Is it in stock?
    /// </summary>
    public bool InStock { get; set; }

    /// <summary>
    /// Quantity in the buyer's shopping cart
    /// </summary>
    public int Amount { get; set; }
}
