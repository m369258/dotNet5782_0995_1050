using DalApi;
using System.Security.Principal;

namespace Dal;

sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();

    private DalList() { }

    public IOrder order => new DalOrder();

    public IProduct product => new DalProduct();

    public IOrderItems orderItems => new DalOrderItem();
}
