using MentalMonitor.Data;
using MentalMonitor.Models;
using MentalMonitor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MentalMonitor.Controllers;

[Authorize]
public class EntryController : Controller
{
    private readonly AppDbContext _db;
    private readonly EmotionService _emotion;
    private readonly AlertService _alert;

    public EntryController(AppDbContext db, EmotionService emotion, AlertService alert)
    {
        _db = db; _emotion = emotion; _alert = alert;
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return View();

        var entry = new Entry
        {
            Text = text,
            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!
        };
        _db.Entries.Add(entry);
        await _db.SaveChangesAsync();

        var scores = await _emotion.AnalyzeEntryAsync(entry);
        _db.EmotionScores.AddRange(scores);
        await _db.SaveChangesAsync();

        var alerts = await _alert.ScanNewEntryAsync(entry, scores);
        if (alerts.Any())
        {
            _db.Alerts.AddRange(alerts);
            await _db.SaveChangesAsync();
        }

        return RedirectToAction("Index", "Emotion");
    }
}