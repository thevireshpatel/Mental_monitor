using MentalMonitor.Data;
using MentalMonitor.Models;
using Microsoft.EntityFrameworkCore;

namespace MentalMonitor.Services;
public class AlertService
{
    private readonly AppDbContext _db;
    public AlertService(AppDbContext db) => _db = db;

    public async Task<List<Alert>> ScanNewEntryAsync(Entry entry, List<EmotionScore> scores)
    {
        var alerts = new List<Alert>();

        // any negative emotion > 25 %
        var negative = new[] { "sadness", "anger", "fear", "disappointment", "disgust", "grief", "remorse" };
        foreach (var s in scores.Where(x => negative.Contains(x.EmotionLabel) && x.Score > 0.25))
        {
            alerts.Add(new Alert
            {
                EntryId = entry.Id,
                Level = AlertLevel.Warning,
                Message = $"High {s.EmotionLabel} detected ({s.Score:0%})"
            });
        }

        // trend: last 3 entries average sadness > 25 %
        var recent = await _db.Entries
                              .Where(e => e.UserId == entry.UserId && e.Id != entry.Id)
                              .OrderByDescending(e => e.CreatedAt)
                              .Take(2)
                              .SelectMany(e => e.EmotionScores)
                              .Where(es => es.EmotionLabel == "sadness")
                              .ToListAsync();

        recent.AddRange(scores.Where(s => s.EmotionLabel == "sadness"));
        if (recent.Count >= 3 && recent.Average(s => s.Score) > 0.25)
        {
            alerts.Add(new Alert
            {
                EntryId = entry.Id,
                Level = AlertLevel.Critical,
                Message = "Negative trend: sustained sadness detected"
            });
        }

        return alerts;
    }
}