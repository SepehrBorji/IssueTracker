using System;
using IssueTracker.ConsoleApp.Data;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Ensure the database exists and is up to date with the latest migrations.
            using (var db = new IssueTrackerContext())
            {
                db.Database.Migrate();
            }

            Console.WriteLine("Database is ready. (IssueTracker.db)");
        }

    }
}
