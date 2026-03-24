namespace ServiceDesk.API.Models;
public enum TicketStatus
{
    Open,
    InProgress,
    WaitingUser,
    Resolved,
    Closed
}
public enum TicketPriority
{
    Low,
    Medium,
    High,
    Critical
}
public class Ticket
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TicketStatus Status { get; set; } = TicketStatus.Open;
    public TicketPriority Priority { get; set; } = TicketPriority.Medium;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }

    // Clés étrangères (Foreign Keys)
    public Guid CreatedById { get; set; }
    public Guid? AssignedToId { get; set; }
    public Guid CategoryId { get; set; }

    // Navigation properties
    public User CreatedBy { get; set; } = null!;
    public User? AssignedTo { get; set; }
    public Category Category { get; set; } = null!;
    public ICollection<Comment> Comments { get; set; } = [];
    public ICollection<TicketHistory> History { get; set; } = [];
}