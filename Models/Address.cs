#nullable disable
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;
public class Address
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid CityId { get; set; }

    [NotMapped]
    public string FullAddress => $"м. {City.Name}, {Name}";

    [ForeignKey("CityId")]
    public virtual City City { get; set; }
}
