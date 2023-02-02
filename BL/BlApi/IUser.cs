namespace BlApi;

public interface IUser
{
    /// <summary>
    /// Add an user
    /// </summary>
    /// <param name="user">user to add</param>
    /// <returns></returns>
    public void AddUser(BO.Users user);

    /// <summary>
    /// user request
    /// </summary>
    /// <param name="idUser">id of user to return</param>
    /// <returns>user </returns>
    public BO.Users GetUser(int idUser);

    /// <summary>
    /// list of users
    /// </summary>
    /// <returns> list of users</returns>
    public IEnumerable<BO.Users> GetListOfUsers();


}
