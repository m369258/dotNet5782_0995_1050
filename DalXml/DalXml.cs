using DalApi;
namespace Dal;

sealed internal class DalXml : IDal
{
    private DalXml() { }
    public static IDal Instance { get; } = new DalXml();
    public IProduct product { get; } = new Dal.Product();
    public IOrder order { get; } = new Dal.Order();
    public IOrderItems orderItems { get; } = new Dal.OrderItem();
    public IUser users { get; } = new Dal.User();
}
