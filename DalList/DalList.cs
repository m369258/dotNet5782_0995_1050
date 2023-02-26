using DalApi;
namespace Dal;

sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();

    private DalList() { }

    public IOrder order => new DalOrder();

    public IProduct product => new DalProduct();

    public IOrderItems orderItems => new DalOrderItem();

    public IUser users => new DalUser();

}
