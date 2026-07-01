namespace CareerPlatform.Domain.Entities;

public class StudentProfile
{
    public Guid ProfileId { get; set; }

    public Guid UserId { get; set; }

    public string? University { get; set; }

    public string? Major { get; set; }

    public decimal? GPA { get; set; }

    public string? GithubUrl { get; set; }

    public User User { get; set; } = null!;
}
