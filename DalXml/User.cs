using DalApi;
using Do;
using System.Xml.Linq;
namespace Dal;

internal class User : IUser
{
    const string s_users = @"User";
    //public int Add(Users entity)
    //{
    //    entity.ID = Tools.getNextID(@"NextUserId");
    //    XElement userRootElem = Tools.LoadListFromXMLElement(s_users);
    //    XElement? user = (from us in userRootElem.Elements()
    //                      where Convert.ToInt32(us.Element("ID").Value) == entity.ID
    //                      select us).FirstOrDefault();

    //    if (user != null)
    //        throw new Do.DalAlreadyExistsException(entity.ID, "id is already exsist");

    //    XElement userElement = new XElement("user",
    //        new XElement("ID", entity.ID),
    //        new XElement("Name", entity.Name),
    //        new XElement("Address", entity.Address),
    //                    new XElement("Email", entity.Email),
    //        new XElement("Password", entity.Password),
    //        new XElement("TypeOfUser", entity.TypeOfUser));
    //    userRootElem.Add(userElement);
    //    Tools.SaveListFromXMLElement(userRootElem, s_users);
    //    return entity.ID;

    //}


    public int Add(Users entity)
    {
        entity.ID = Tools.getNextID(@"NextUserId");
        XElement userRootElem = Tools.LoadListFromXMLElement(s_users);
        XElement? user = (from us in userRootElem.Elements()
                          where (us.Element("Email").Value) == entity.Email
                          select us).FirstOrDefault();

        if (user != null)
            throw new Do.DalAlreadyExistsException(entity.ID, "id is already exsist");

        XElement userElement = new XElement("user",
            new XElement("ID", entity.ID),
            new XElement("Name", entity.Name),
            new XElement("Address", entity.Address),
                        new XElement("Email", entity.Email),
            new XElement("Password", entity.Password),
            new XElement("TypeOfUser", entity.TypeOfUser));
        userRootElem.Add(userElement);
        Tools.SaveListFromXMLElement(userRootElem, s_users);
        return entity.ID;

    }

    static Do.Users? creatUserFromXElement(XElement xUser)
    {
        Do.TypeOfUser t;
        Enum.TryParse((string?)(xUser.Element("TypeOfUser")), out t);
        return new Do.Users()
        {
            ID = xUser.ToIntNullable("ID") ?? throw new Do.DalDoesNotExistException("id doesnt exsist"),
            Name = (string?)xUser.Element("Name"),
            Address = (string?)xUser.Element("Address"),
            Email = (string?)xUser.Element("Email"),
            Password = (string?)xUser.Element("Password"),
            TypeOfUser = t
        };
    }

    public Do.Users Get(Func<Users?, bool> condition)
    {
        IEnumerable<Do.Users?> users = new List<Do.Users?>();
        XElement userRootElem = Tools.LoadListFromXMLElement(s_users);

        if (condition != null)
            users = (from s in userRootElem.Elements()
                     select creatUserFromXElement(s)).ToList();

        return users.FirstOrDefault(myuser => condition(myuser)) ??
     throw new Do.DalDoesNotExistException("there are no user with this id");
    }

    public IEnumerable<Do.Users?> GetAll(Func<Do.Users?, bool>? condition = null)
    {
        XElement userRootElem = Tools.LoadListFromXMLElement(s_users);
        if (condition != null)
            return from s in userRootElem.Elements()
                   let user = creatUserFromXElement(s)
                   where condition(user)
                   select user;
        else
            return from s in userRootElem.Elements() select creatUserFromXElement(s);
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
