namespace DalApi;

public interface IDal
{
    IProduct product { get; }
    IOrder order { get; }
    IOrderItems orderItems { get; }
    IUser users { get; }

}
