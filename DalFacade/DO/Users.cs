using DO;
namespace Do;

public struct Users
{
    public int ID { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }

    public string? Password { get; set; }

    public Do.TypeOfUser TypeOfUser { get; set; }

    public override string ToString() => this.ToStringProperty();
}
