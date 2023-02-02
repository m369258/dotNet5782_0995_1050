namespace BO;

public class Users
{
    public int ID { get; set; }
    public string? Email { get; set; }

    public string? Password { get; set; }

    public BO.TypeOfUser TypeOfUser { get; set; }

    public override string ToString() => this.ToStringProperty();
}
