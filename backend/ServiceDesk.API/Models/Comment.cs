namespace ServiceDesk.API.Models;

public class Comment
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsInternal { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Clés étrangères
    public Guid TicketId { get; set; }
    public Guid AuthorId { get; set; }

    // Navigation properties
    public Ticket Ticket { get; set; } = null!;
    public User Author { get; set; } = null!;
}