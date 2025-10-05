using System.IO;
using IssueTracker.ConsoleApp.Models;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.ConsoleApp.Data
{
    public class IssueTrackerContext : DbContext
    {
        public DbSet<Issue> Issues { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Database file in the app’s working directory:
            string dbPath = Path.Combine(Directory.GetCurrentDirectory(), "IssueTracker.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Data annotations on Issue (Required/MaxLength) are picked up automatically.
            // Put any custom Fluent API here later if needed.
        }
    }
}