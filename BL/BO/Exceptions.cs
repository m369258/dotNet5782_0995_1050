
namespace BO;


public class NotEnoughInStock : Exception
{
    public NotEnoughInStock(string? message) : base(message) { }
}
