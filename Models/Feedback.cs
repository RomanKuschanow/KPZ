#nullable disable

namespace Models;
public class Feedback
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Content { get; set; }
    public int Rate { get; set; }
}
