using MentalMonitor.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

namespace MentalMonitor.Controllers;

[Authorize]
public class EmotionController : Controller
{
    private readonly AppDbContext _db;
    public EmotionController(AppDbContext db) => _db = db;

    // Existing chart/table page
    public async Task<IActionResult> Index()
    {
        var uid = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)!;
        var entries = await _db.Entries
                               .Include(e => e.EmotionScores)
                               .Where(e => e.UserId == uid)
                               .OrderByDescending(e => e.CreatedAt)
                               .Take(20)
                               .ToListAsync();
        return View(entries);
    }

    // NEW: CSV export
    [Authorize]
    public async Task<IActionResult> ExportCsv()
    {
        var uid = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var data = await _db.Entries
                            .Where(e => e.UserId == uid)
                            .OrderByDescending(e => e.CreatedAt)
                            .Select(e => new
                            {
                                e.CreatedAt,
                                e.Text,
                                TopEmotion = e.EmotionScores
                                              .OrderByDescending(s => s.Score)
                                              .First().EmotionLabel,
                                TopScore = e.EmotionScores
                                              .OrderByDescending(s => s.Score)
                                              .First().Score
                            })
                            .ToListAsync();

        var csv = new StringBuilder();
        csv.AppendLine("Date,Text,TopEmotion,TopScore");
        foreach (var row in data)
        {
            csv.AppendLine($"\"{row.CreatedAt:yyyy-MM-dd HH:mm}\",\"{row.Text.Replace("\"", "\"\"")}\",{row.TopEmotion},{row.TopScore:0.00}");
        }

        return File(Encoding.UTF8.GetBytes(csv.ToString()),
                    "text/csv",
                    $"MentalMonitor_Export_{DateTime.Now:yyyyMMdd}.csv");
    }
}