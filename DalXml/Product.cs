using DalApi;
using Do;

namespace Dal;

internal class Product : IProduct
{
    public int Add(Do.Product entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Do.Product Get(Func<Do.Product?, bool> condition)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Do.Product?> GetAll(Func<Do.Product?, bool>? condition = null)
    {
        throw new NotImplementedException();
    }

    public void Update(Do.Product updateEntity)
    {
        throw new NotImplementedException();
    }
}
