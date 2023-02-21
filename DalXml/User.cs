using DalApi;
using Do;

namespace Dal;

internal class User : IUser
{
    public int Add(Users entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Users Get(Func<Users?, bool> condition)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Users?> GetAll(Func<Users?, bool>? condition = null)
    {
        throw new NotImplementedException();
    }

    public void Update(Users updateEntity)
    {
        throw new NotImplementedException();
    }
}
