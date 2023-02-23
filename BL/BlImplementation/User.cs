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
        IEnumerable<Do.Users?> users = myDal.users.GetAll();
        Do.Users? isExsistUser = null;
        isExsistUser = users.FirstOrDefault(item => item?.Email == user.Email);
        if (isExsistUser != null)
            throw new BO.AlreadyExsist("You are registered in the system");

        string pass = user.Password ?? throw new InvalidInputException("no password");

        if (!(pass.Any(c => char.IsLetter(c)) && pass.Any(c => char.IsDigit(c) && pass.Length >= 6 && pass.Length <= 10)))
            throw new InvalidArgumentException("the password invalid");

        if (!user.Email!.Contains("@") || !user.Email.Contains("."))
            throw new BO.InvalidArgumentException("Invalid email");

        Do.Users myUser = new Do.Users()
        {
            ID = user.ID,
            Name = user.Name,
            Address = user.Address,
            Email = user.Email,
            Password = user.Password,
            TypeOfUser = Do.TypeOfUser.customer
        };

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
            Name = ((Do.Users)item!).Name,
            Address = ((Do.Users)item!).Address,
            Password = ((Do.Users)item!).Password,
            Email = ((Do.Users)item!).Email,
            TypeOfUser = (BO.TypeOfUser)(((Do.Users)item!).TypeOfUser)!
        });

    }

    public BO.Users GetUser(string email, string password)
    {
        Do.Users? user;

        try { user = myDal.users.Get(item => item?.Email == email && item?.Password == password); }
        catch (Do.DalDoesNotExistException ex) { throw new BO.InternalErrorException("this email doesnt exsist", ex); }

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

