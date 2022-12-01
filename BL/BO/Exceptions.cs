namespace BO;

/// <summary>
/// Throw an error in case the field is invalid - Invalid Argument 
/// </summary>
public class InvalidArgumentException : Exception
{
    public InvalidArgumentException(string? message) : base(message) { }
}

/// <summary>
/// Invalid input
/// </summary>
public class InvalidInputException : Exception
{
    public InvalidInputException(string? message) : base(message) { }
}

/// <summary>
/// internal error
/// </summary>
public class InternalErrorException : Exception
{
    public InternalErrorException(string? message) : base(message) { }
}

/// <summary>
/// Not Enough In Stock Exception
/// </summary>
public class NotEnoughInStockException : Exception
{
    public NotEnoughInStockException(string? message) : base(message) { }
}
