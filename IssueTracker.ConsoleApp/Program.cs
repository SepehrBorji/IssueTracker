using System;
using System.Collections.Generic;
using IssueTracker.ConsoleApp.Data;
using IssueTracker.ConsoleApp.Models;
using IssueTracker.ConsoleApp.Services;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var db = new IssueTrackerContext())
            {
                // Applies pending migrations automatically
                db.Database.Migrate();

                var service = new IssueService(db);
                RunMenu(service);
            }
        }

        private static void RunMenu(IssueService service)
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("==== SquashIt - A Bug Tracking app ====");
                Console.WriteLine("1) Add Issue");
                Console.WriteLine("2) List All Issues");
                Console.WriteLine("3) List By Type");
                Console.WriteLine("4) List By Status");
                Console.WriteLine("5) List By Priority Level");
                Console.WriteLine("6) Update Issue Status");
                Console.WriteLine("7) Update Issue Description");
                Console.WriteLine("8) Update Issue Priority Level");
                Console.WriteLine("9) Delete Issue");
                Console.WriteLine("10) Delete all issues");
                Console.WriteLine("0) Exit");
                Console.Write("Choose: ");

                var choice = Console.ReadLine();
                Console.WriteLine();

                if (choice == "0")
                {
                    return;
                }

                if (choice == "1")
                {
                    DoAddIssue(service);
                }
                else if (choice == "2")
                {
                    DoListIssues(service.GetAllIssues());
                }
                else if (choice == "3")
                {
                    var type = PromptIssueType();
                    DoListIssues(service.GetIssuesByType(type));
                }
                else if (choice == "4")
                {
                    var status = PromptIssueStatus();
                    DoListIssues(service.GetIssuesByStatus(status));
                }
                else if (choice == "5")
                {
                    var priority = PromptIssuePriority();
                    DoListIssues(service.GetIssuesByPriority(priority));
                }
                else if (choice == "6")
                {
                    DoUpdateIssueStatus(service);
                }
                else if (choice == "7")
                {
                    DoUpdateIssueDescription(service);
                }
                else if (choice == "8")
                {
                    DoUpdateIssuePriority(service);
                }
                else if (choice == "9")
                {
                    DoDeleteIssue(service);
                }
                else if (choice == "10")
                {
                    DoDeleteAllIssues(service);
                }
            }
        }

        private static void DoAddIssue(IssueService service)
        {
            Console.Write("Title: ");
            var title = ReadNonEmpty();


            var desc = PromptIssueDescription();
            var priority = PromptIssuePriority();
            var status = PromptIssueStatus();
            var type = PromptIssueType();

            var id = service.AddIssue(title, desc, priority, type, status);
            Console.WriteLine("Added issue with ID: " + id);
        }

        private static void DoListIssues(List<Issue> issues)
        {
            if (issues.Count == 0)
            {
                Console.WriteLine("No issues found.");
                return;
            }

            foreach (var issue in issues)
            {
                Console.WriteLine(FormatIssueLine(issue));
            }
        }

        private static string FormatIssueLine(Issue issue)
        {
            var created = issue.CreatedAt.ToString("yyyy-MM-dd HH:mm");
            var updated = issue.UpdatedAt.HasValue ? issue.UpdatedAt.Value.ToString("yyyy-MM-dd HH:mm") : "-";
            return "#" + issue.Id + " [" + issue.Type + "] " + issue.Title + " — " + issue.Status
                   + " | Created: " + created + " | Updated: " + updated + "\n\t" + issue.Description;
        }

        private static void DoUpdateIssueDescription(IssueService service)
        {
            List<Issue> issues = service.GetAllIssues();
            if (issues.Count == 0)
            {
                Console.WriteLine("No issues found.");
                return;
            }

            DoListIssues(issues);

            Console.Write("Enter issue ID to update its description: ");
            var id = ReadInt();

            var newDescription = PromptIssueDescription();

            var ok = service.UpdateIssueDescription(id, newDescription);
            if (ok)
            {
                Console.WriteLine("Description updated.");
            }
            else
            {
                Console.WriteLine("Issue not found.");
            }
        }

        private static void DoUpdateIssuePriority(IssueService service)
        {
            List<Issue> issues = service.GetAllIssues();
            if (issues.Count == 0)
            {
                Console.WriteLine("No issues found.");
                return;
            }

            DoListIssues(issues);

            Console.Write("Enter issue ID to update its priority level: ");
            var id = ReadInt();

            var newPriority = PromptIssuePriority();

            var ok = service.UpdateIssuePriority(id, newPriority);
            if (ok)
            {
                Console.WriteLine("Status updated.");
            }
            else
            {
                Console.WriteLine("Issue not found.");
            }
        }

        private static void DoUpdateIssueStatus(IssueService service)
        {
            List<Issue> issues = service.GetAllIssues();
            if (issues.Count == 0)
            {
                Console.WriteLine("No issues found.");
                return;
            }

            DoListIssues(issues);

            Console.Write("Enter issue ID to update its status: ");
            var id = ReadInt();

            var newStatus = PromptIssueStatus();

            var ok = service.UpdateIssueStatus(id, newStatus);
            if (ok)
            {
                Console.WriteLine("Status updated.");
            }
            else
            {
                Console.WriteLine("Issue not found.");
            }
        }

        private static void DoDeleteIssue(IssueService service)
        {
            List<Issue> issues = service.GetAllIssues();
            if (issues.Count == 0)
            {
                Console.WriteLine("No issues found.");
                return;
            }

            DoListIssues(issues);

            Console.Write("Enter Issue ID to delete: ");
            var id = ReadInt();

            Console.Write("Are you sure? (y/N): ");
            var confirm = Console.ReadLine();
            if (confirm != null && confirm.Trim().ToLower() == "y")
            {
                var ok = service.DeleteIssue(id);
                if (ok)
                {
                    Console.WriteLine("Issue deleted.");
                }
                else
                {
                    Console.WriteLine("Issue not found.");
                }
            }
            else
            {
                Console.WriteLine("Cancelled.");
            }
        }

        private static void DoDeleteAllIssues(IssueService service)
        {
            List<Issue> issues = service.GetAllIssues();
            if (issues.Count == 0)
            {
                Console.WriteLine("No issues found.");
                return;
            }

            DoListIssues(issues);

            Console.Write("Are you sure? (y/N): ");
            var confirm = Console.ReadLine();
            if (confirm != null && confirm.Trim().ToLower() == "y")
            {
                issues.Clear();
                Console.WriteLine("All issues have been deleted");
            }
            else
            {
                Console.WriteLine("Operation Cancelled.");
            }
        }

        private static IssueType PromptIssueType()
        {
            Console.WriteLine("Issue Type:");
            Console.WriteLine("0) Bug");
            Console.WriteLine("1) Feature");
            Console.WriteLine("2) Task");
            Console.Write("Choose: ");
            var value = ReadInt();

            if (value < 0 || value > 2)
            {
                Console.WriteLine("Invalid choice. Defaulting to Task.");
                return IssueType.Task;
            }

            return (IssueType)value;
        }

        private static string PromptIssueDescription()
        {
            Console.Write("Description (optional): ");
            var value = Console.ReadLine();
            return value;
        }

        private static IssuePriority PromptIssuePriority()
        {
            Console.WriteLine("Issue Priority Level: ");
            Console.WriteLine("0) High");
            Console.WriteLine("1) Medium");
            Console.WriteLine("2) Low");
            Console.Write("Choose: ");

            var value = ReadInt();

            if (value < 0 || value > 2)
            {
                Console.WriteLine("Invalid choice. Defaulting to High.");
                return IssuePriority.High;
            }

            return (IssuePriority)value;
        }

        private static IssueStatus PromptIssueStatus()
        {
            Console.WriteLine("Issue Status:");
            Console.WriteLine("0) Open");
            Console.WriteLine("1) InProgress");
            Console.WriteLine("2) Closed");
            Console.Write("Choose: ");
            var value = ReadInt();

            if (value < 0 || value > 2)
            {
                Console.WriteLine("Invalid choice. Defaulting to Open.");
                return IssueStatus.Open;
            }

            return (IssueStatus)value;
        }

        private static string ReadNonEmpty()
        {
            while (true)
            {
                var s = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(s))
                {
                    return s.Trim();
                }
                Console.Write("Please enter a non-empty value: ");
            }
        }

        private static int ReadInt()
        {
            while (true)
            {
                var s = Console.ReadLine();
                int v;
                if (int.TryParse(s, out v))
                {
                    return v;
                }
                Console.Write("Please enter a valid number: ");
            }
        }
    }
}
