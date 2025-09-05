using System.ComponentModel.DataAnnotations;

namespace MentalMonitor.Models;
public class EmotionScore
{
    [Key] public int Id { get; set; }

    [Required, MaxLength(50)] public string EmotionLabel { get; set; } = default!;
    public double Score { get; set; }   // 0-1 probability
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int EntryId { get; set; }
    public Entry Entry { get; set; } = default!;
}