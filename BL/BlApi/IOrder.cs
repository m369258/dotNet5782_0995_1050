namespace BlApi;

public interface IOrder
{
    public IEnumerable<BO.OrderForList> GetListOfOrders();

}
