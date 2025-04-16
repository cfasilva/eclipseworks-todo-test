namespace Eclipseworks.Domain.Models;

public class TaskHistoryModel
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public int ModifiedByUserId { get; set; }
    public string FieldChanged { get; set; } = "";
    public string OldValue { get; set; } = "";
    public string NewValue { get; set; } = "";
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
}

