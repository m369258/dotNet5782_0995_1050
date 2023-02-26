using Do;
namespace Dal;
using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

internal class DalUser : IUser
{
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Add(Users user)
    {
        int i = 0;
        for (i = 0; i < DataSource.users.Count && (DataSource.users[i]?.Email != user.Email); i++) ;
        if (i != DataSource.users.Count)
            throw new Do.DalAlreadyExistsException(user.ID, "user", "this email user is exsist");
        DataSource.users.Add(user);
        return user.ID;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public Users Get(Func<Users?, bool> condition)
    {
        return DataSource.users.FirstOrDefault(myuser => condition(myuser)) ??
      throw new Do.DalDoesNotExistException("there are no user with this id");
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Users?> GetAll(Func<Users?, bool>? condition = null)
    {
        return condition != null ?
                DataSource.users.Where(currUser => condition(currUser)) :
                DataSource.users.Select(currUser => currUser);
    }

    public void Update(Users updateEntity)
    {
        throw new NotImplementedException();
    }
    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

}
