using System;
using System.Collections.Generic;
using System.Linq;
using IssueTracker.ConsoleApp.Data;
using IssueTracker.ConsoleApp.Models;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.ConsoleApp.Services
{
    public class IssueService
    {
        private readonly IssueTrackerContext _db;

        public IssueService(IssueTrackerContext db)
        {
            _db = db;
        }

        public int AddIssue(string title, string description, IssueType type)
        {
            var issue = new Issue();
            issue.Title = title;
            issue.Description = string.IsNullOrWhiteSpace(description) ? null : description;
            issue.Type = type;
            issue.Status = IssueStatus.Open;
            issue.CreatedAt = DateTime.Now;

            _db.Issues.Add(issue);
            _db.SaveChanges();
            return issue.Id;
        }

        public List<Issue> GetAllIssues()
        {
            // Simple order: newest first
            return _db.Issues
                      .OrderByDescending(i => i.CreatedAt)
                      .AsNoTracking()
                      .ToList();
        }

        public List<Issue> GetIssuesByType(IssueType type)
        {
            return _db.Issues
                      .Where(i => i.Type == type)
                      .OrderByDescending(i => i.CreatedAt)
                      .AsNoTracking()
                      .ToList();
        }

        public List<Issue> GetIssuesByStatus(IssueStatus status)
        {
            return _db.Issues
                      .Where(i => i.Status == status)
                      .OrderByDescending(i => i.CreatedAt)
                      .AsNoTracking()
                      .ToList();
        }

        public List<Issue> GetIssuesByPriority(IssuePriority priorityLevel)
        {
            return _db.Issues
                      .Where(i => i.PriorityLevel == priorityLevel)
                      .OrderByDescending(i => i.CreatedAt)
                      .AsNoTracking()
                      .ToList();
        }

        public bool UpdateIssueStatus(int id, IssueStatus newStatus)
        {
            var issue = _db.Issues.FirstOrDefault(i => i.Id == id);
            if (issue == null)
            {
                return false;
            }

            issue.Status = newStatus;
            issue.UpdatedAt = DateTime.Now;
            _db.SaveChanges();
            return true;
        }

        public bool DeleteIssue(int id)
        {
            var issue = _db.Issues.FirstOrDefault(i => i.Id == id);
            if (issue == null)
            {
                return false;
            }

            _db.Issues.Remove(issue);
            _db.SaveChanges();
            return true;
        }
    }
}
