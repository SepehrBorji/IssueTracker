using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Api.Entities;

public class Issue
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(4000)]
    public string? Description { get; set; }

    public IssueType Type { get; set; } = IssueType.Task;

    public IssueStatus Status { get; set; } = IssueStatus.Open;
    public IssuePriority PriorityLevel { get; set; } = IssuePriority.High;

    /// <summary>
    /// Local timestamp when the issue was created (uses machine local time).
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Local timestamp when the issue was last updated (uses machine local time).
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    public Issue()
    {
        // Use local time (PST/PDT on your machine in Vancouver).
        CreatedAt = DateTime.Now;
    }
    public override string ToString()
    {
        return $"#{Id} [{Type}] {Title} — {Status}";
    }
}
