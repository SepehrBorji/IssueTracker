using IssueTracker.Api.Entities;

namespace IssueTracker.Api.DTOs
{
    public class CreateIssueDTO
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public IssuePriority Priority { get; set; }
        public IssueType Type { get; set; }
        public IssueStatus Status { get; set; } = IssueStatus.Open;
    }
}
