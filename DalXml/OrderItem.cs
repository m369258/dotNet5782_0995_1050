using DalApi;
using Do;

namespace Dal;

internal class OrderItem : IOrderItems
{
    public int Add(Do.OrderItem entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Do.OrderItem Get(Func<Do.OrderItem?, bool> condition)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Do.OrderItem?> GetAll(Func<Do.OrderItem?, bool>? condition = null)
    {
        throw new NotImplementedException();
    }

    public void Update(Do.OrderItem updateEntity)
    {
        throw new NotImplementedException();
    }
}
