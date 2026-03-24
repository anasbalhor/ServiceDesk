namespace ServiceDesk.API.Models;

public class TicketHistory
{
    public Guid Id { get; set; }
    public string FieldChanged { get; set; } = string.Empty;
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public DateTime ChangedAt { get; set; }

    // Clés étrangères
    public Guid TicketId { get; set; }
    public Guid ChangedById { get; set; }

    // Navigation properties
    public Ticket Ticket { get; set; } = null!;
    public User ChangedBy { get; set; } = null!;
}
