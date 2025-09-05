using System.ComponentModel.DataAnnotations;

namespace MentalMonitor.Models;
public class Entry
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(2000)]
    public string Text { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public ICollection<EmotionScore> EmotionScores { get; set; } = new List<EmotionScore>();
    public ICollection<Alert> Alerts { get; set; } = new List<Alert>();
}