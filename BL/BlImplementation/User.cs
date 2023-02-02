using BlApi;
using BO;
using DalApi;
using Do;

namespace BlImplementation;

internal class User : BlApi.IUser
{
    DalApi.IDal myDal = DalApi.Factory.Get();

    public void AddUser(BO.Users user)
    {
        //בדיקה שהמייל לא קיים

        string pass = user.Password ?? throw new InvalidInputException("no password");

        if (!(pass.Any(c => char.IsLetter(c)) && pass.Any(c => char.IsDigit(c) && pass.Length >= 6 && pass.Length <= 10)))
            throw new InvalidArgumentException("the password invalid");

        if (!user.Email!.Contains("@") || !user.Email.Contains("."))
            throw new BO.InvalidArgumentException("Invalid email");

        Do.Users myUser = new Do.Users();
        myUser.CopyBetweenEnriries(user);

        try
        {
            myDal.users.Add(myUser);
        }
        catch (Do.DalAlreadyExistsException) { throw new AlreadyExsist("the user is exsis"); }

    }

    public IEnumerable<BO.Users> GetListOfUsers()
    {
        IEnumerable<Do.Users?> doUsers;

        doUsers = myDal.users.GetAll();

        return doUsers.Select(item => (item == null) ? throw new BO.InternalErrorException("Data layer item does not exist") : new BO.Users()
        {
            ID = ((Do.Users)item!).ID,
            Password = ((Do.Users)item!).Password,
            Email = ((Do.Users)item!).Email,
            TypeOfUser = (BO.TypeOfUser)(((Do.Users)item!).TypeOfUser)!
        });

    }

    public BO.Users GetUser(int idUser)
    {
        Do.Users? user;
        try { user = myDal.users.Get(item => item?.ID == idUser); }
        catch (Do.DalDoesNotExistException ex) { throw new BO.InternalErrorException("this id doesnt exsist", ex); }

        //Building a new object from the display product type
        BO.Users boUser = new BO.Users();

        //Sending to an extension function that converts an object to be of type display layer.
        user?.CopyBetweenEnriries(boUser);

        //Copying mismatched fields
        boUser.TypeOfUser = (BO.TypeOfUser)(user?.TypeOfUser)!;
        return boUser;
    }

    static bool IsLetter(char c)
    {
        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
    }

    static bool IsDigit(char c)
    {
        return c >= '0' && c <= '9';
    }
}

