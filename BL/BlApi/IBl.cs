using System.Security.Principal;

namespace BlApi;

public interface IBl
{
    public IOrder order { get; }
    public IProduct product { get; }
    public ICart cart { get; }

    public IUser user { get; }

}
