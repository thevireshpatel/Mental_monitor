using System.ComponentModel.DataAnnotations;

namespace MentalMonitor.Models;
public enum AlertLevel { Info, Warning, Critical }

public class Alert
{
    [Key] public int Id { get; set; }
    public AlertLevel Level { get; set; }

    [MaxLength(500)] public string Message { get; set; } = default!;
    public bool Dismissed { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int EntryId { get; set; }
    public Entry Entry { get; set; } = default!;
}