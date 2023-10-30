#nullable disable

namespace Models;
public class Owner
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Telephone { get; set; }
    public string Email { get; set; }

    public override string ToString() => Name;
}
