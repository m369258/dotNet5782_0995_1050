using System.Security.Principal;

namespace BlApi;
sealed internal class Bl : IBl
{
    public BlApi.IOrder order => new BlImplementation.Order();

    public IProduct product => new BlImplementation.Product();

    public ICart cart => new BlImplementation.Cart();
}
