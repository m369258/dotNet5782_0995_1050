
using Do;

namespace BO;

/// <summary>
/// The product list screen and a catalog screen, which will contain the following.
/// </summary>
public class ProductForList
{
    /// <summary>
    /// ID (product)
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// product name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// product price
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// category
    /// </summary>
    public Category category { get; set; }
}
