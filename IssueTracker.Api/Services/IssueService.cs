using System;
using System.Collections.Generic;
using System.Linq;
using IssueTracker.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Api.Services
{
    public class IssueService
    {
        private readonly IssueTrackerContext _db;

        public IssueService(IssueTrackerContext db)
        {
            _db = db;
        }

        public async Task<int> AddIssueAsync(string title, string description, IssuePriority priority,
                                        IssueType type, IssueStatus status = IssueStatus.Open)
        {
            var issue = new Issue
            {
                Title = title,
                Description = string.IsNullOrWhiteSpace(description) ? null : description,
                Type = type,
                Status = status,
                PriorityLevel = priority,
                CreatedAt = DateTime.Now
            };

            await _db.Issues.AddAsync(issue);
            await _db.SaveChangesAsync();

            return issue.Id;
        }


        public async Task<List<Issue>> GetAllIssuesAsync()
        {
            return await _db.Issues
                .OrderByDescending(i => i.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Issue>> GetIssuesByTypeAsync(IssueType type)
        {
            return await _db.Issues
                .Where(i => i.Type == type)
                .OrderByDescending(i => i.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Issue>> GetIssuesByStatusAsync(IssueStatus status)
        {
            return await _db.Issues
                .Where(i => i.Status == status)
                .OrderByDescending(i => i.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Issue>> GetIssuesByPriorityAsync(IssuePriority priorityLevel)
        {
            return await _db.Issues
                .Where(i => i.PriorityLevel == priorityLevel)
                .OrderByDescending(i => i.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> UpdateIssueStatusAsync(int id, IssueStatus newStatus)
        {
            var issue = await _db.Issues.FirstOrDefaultAsync(i => i.Id == id);
            if (issue == null)
            {
                return false;
            }

            issue.Status = newStatus;
            issue.UpdatedAt = DateTime.Now;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateIssueDescriptionAsync(int id, string newDescription)
        {
            var issue = await _db.Issues.FirstOrDefaultAsync(i => i.Id == id);
            if (issue == null)
            {
                return false;
            }

            issue.Description = newDescription;
            issue.UpdatedAt = DateTime.Now;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateIssuePriorityAsync(int id, IssuePriority newPriority)
        {
            var issue = await _db.Issues.FirstOrDefaultAsync(i => i.Id == id);
            if (issue == null)
            {
                return false;
            }

            issue.PriorityLevel = newPriority;
            issue.UpdatedAt = DateTime.Now;

            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteIssueAsync(int id)
        {
            var issue = await _db.Issues.FirstOrDefaultAsync(i => i.Id == id);
            if (issue == null)
            {
                return false;
            }

            _db.Issues.Remove(issue);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
