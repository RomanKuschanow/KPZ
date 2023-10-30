#nullable disable
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;
public class Place
{
    public Guid Id { get; set; }
    public Guid AddressId { get; set; }
    public Guid OwnerId { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsOpen { get; set; }
    public string AddInfo { get; set; }

    [ForeignKey("AddressId")]
    public virtual Address Address { get; set; }
    [ForeignKey("OwnerId")]
    public virtual Owner Owner { get; set; }
}
