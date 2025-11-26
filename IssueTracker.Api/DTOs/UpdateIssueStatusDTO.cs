using IssueTracker.Api.Entities;

namespace IssueTracker.Api.DTOs
{
    public class UpdateIssueStatusDTO
    {
        public IssueStatus Status { get; set; }
    }
}
