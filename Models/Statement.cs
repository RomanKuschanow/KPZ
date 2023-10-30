#nullable disable
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;
public class Statement
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public string Content { get; set; }
    public string Status { get; set; }

    [ForeignKey("OwnerId")]
    public virtual Owner Owner { get; set; }
}
