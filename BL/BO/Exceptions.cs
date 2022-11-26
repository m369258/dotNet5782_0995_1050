namespace BO;

/// <summary>
/// Error if the index is not correct
/// </summary>
public class IncorrectIndex : Exception
{
    public IncorrectIndex(string? message) : base(message) { }
}

/// <summary>
/// Throw an error in case the field is invalid
/// </summary>
public class InvalidField: Exception
{
    public InvalidField(string? message) : base(message) { }
}

public class CannotPerformThisOperation : Exception

public class NotEnoughInStock : Exception
{
    public CannotPerformThisOperation(string? message) : base(message) { }
    public NotEnoughInStock(string? message) : base(message) { }
}
