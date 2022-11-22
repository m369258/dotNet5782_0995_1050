using DalApi;
using System.Security.Principal;

namespace Dal;

sealed public class DalList1 : IDal
{
    public IOrder order => new DalOrder();

    public IProduct product => new DalProduct();

    public IOrderItems orderItems => new DalOrderItem();
}
