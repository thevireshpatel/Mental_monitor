using MentalMonitor.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace MentalMonitor.Data;
public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> o) : base(o) { }

    public DbSet<Entry> Entries => Set<Entry>();
    public DbSet<EmotionScore> EmotionScores => Set<EmotionScore>();
    public DbSet<Alert> Alerts => Set<Alert>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);
        b.Entity<EmotionScore>().HasOne(e => e.Entry).WithMany(e => e.EmotionScores).OnDelete(DeleteBehavior.Cascade);
        b.Entity<Alert>().HasOne(a => a.Entry).WithMany(e => e.Alerts).OnDelete(DeleteBehavior.Cascade);
    }
}