using MentalMonitor.Data;
using MentalMonitor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MentalMonitor.Controllers;

[Authorize]
public class AlertController : Controller
{
    private readonly AppDbContext _db;
    public AlertController(AppDbContext db) => _db = db;

    // Existing index page
    public async Task<IActionResult> Index()
    {
        var uid = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var alerts = await _db.Alerts
                              .Include(a => a.Entry)
                              .Where(a => a.Entry.UserId == uid && !a.Dismissed)
                              .OrderByDescending(a => a.CreatedAt)
                              .ToListAsync();
        return View(alerts);
    }

    // AJAX badge count
    public async Task<IActionResult> BadgeCount()
    {
        var uid = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var count = await _db.Alerts
                             .Include(a => a.Entry)
                             .CountAsync(a => a.Entry.UserId == uid && !a.Dismissed);
        return Json(count);
    }

    [HttpPost]
    public async Task<IActionResult> Dismiss(int id)
    {
        var alert = await _db.Alerts.FindAsync(id);
        if (alert != null) { alert.Dismissed = true; await _db.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }
}