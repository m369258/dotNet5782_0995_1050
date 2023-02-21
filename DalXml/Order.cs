using DalApi;
using Do;

namespace Dal;

internal class Order : IOrder
{
    public int Add(Do.Order entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Do.Order Get(Func<Do.Order?, bool> condition)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Do.Order?> GetAll(Func<Do.Order?, bool>? condition = null)
    {
        throw new NotImplementedException();
    }

    public void Update(Do.Order updateEntity)
    {
        throw new NotImplementedException();
    }
}
