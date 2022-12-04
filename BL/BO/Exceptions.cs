namespace BO;

/// <summary>
/// Throw an error in case the field is invalid - Invalid Argument 
/// </summary>
public class InvalidArgumentException : Exception
{
    public InvalidArgumentException() : base() { }
    public InvalidArgumentException(string? message) : base(message) { }
    public InvalidArgumentException(string? message, Exception innerException) : base(message, innerException) { }
    public override string ToString() =>
      $"invalid argument";
}

/// <summary>
/// Invalid input - exception in case a field was invalid 
/// </summary>
public class InvalidInputException : Exception
{
    public string DetailName;
    public InvalidInputException(string name)
        : base() { DetailName = name; }
    public InvalidInputException(string name, string massage)
        : base(massage) { DetailName = name; }
    public InvalidInputException(string name, string massage, Exception innerException)
        : base(massage, innerException) { DetailName = name; }
    public override string ToString() =>
        $"The field {DetailName} is invalid!";
}

/// <summary>
/// internal error - exception in case a product/order/orderItem has been requested in the bl and its id doesn't exist in the dal: 
/// exception in case a product/order/orderItem has been requested to be added to the dal in the bl and its id already exists in the dal
/// </summary>
public class InternalErrorException : Exception
{
    public InternalErrorException(string message)
        : base(message) { }
    public InternalErrorException(string message, Exception innerException)
        : base(message, innerException) { }
    public override string ToString() =>
       $"Missing Entity";
}

/// <summary>
/// Not Enough In Stock Exception - exception in case a product has been requested to be bought/added to cart and it is not in stock 
/// </summary>
public class NotEnoughInStockException : Exception
{
    public int ProductID;
    public string ProductName;
    public NotEnoughInStockException(int id, string name)
        : base() { ProductID = id; ProductName = name; }
    public NotEnoughInStockException(int id, string name, string massage)
        : base(massage) { ProductID = id; ProductName = name; }
    public NotEnoughInStockException(int id, string name, string massage, Exception innerException)
        : base(massage, innerException) { ProductID = id; ProductName = name; }
    public override string ToString() =>
        $"Product {ProductName} number {ProductID}, not enough in stock.";
}
