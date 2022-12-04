namespace BO;

/// <summary>
/// Throw an error in case the field is invalid - Invalid Argument 
/// </summary>
public class InvalidArgumentException : Exception
{
    public InvalidArgumentException() : base() { }
    public InvalidArgumentException(string? message) : base(message) { }
    public InvalidArgumentException(string? message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// Invalid input
/// </summary>
public class InvalidInputException : Exception
{
    public InvalidInputException() : base() { }

    public InvalidInputException(string? message) : base(message) { }
}

/// <summary>
/// internal error
/// </summary>
public class InternalErrorException : Exception
{
    public InternalErrorException() : base() { }
    public InternalErrorException(string? message) : base(message) { }
}

/// <summary>
/// Not Enough In Stock Exception
/// </summary>
public class NotEnoughInStockException : Exception
{
    public NotEnoughInStockException() : base() { }
    public NotEnoughInStockException(string? message) : base(message) { }
}
