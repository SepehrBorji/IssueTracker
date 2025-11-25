using System.IO;
using IssueTracker.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Api.Data
{
    public class IssueTrackerContext : DbContext
    {
        public DbSet<Issue> Issues { get; set; }

        public IssueTrackerContext(DbContextOptions<IssueTrackerContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // optional for fluent api
        }
    }
}