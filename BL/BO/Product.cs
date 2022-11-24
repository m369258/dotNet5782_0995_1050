using Do;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

internal class Product
{
    /// <summary>
    /// Unique ID number (like the barcode number of the product)
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// Product Name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// category
    /// </summary>
    public Category Category { get; set; }

    /// <summary>
    /// price
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// Units in Stock
    /// </summary>
    public int InStock { get; set; }
}
