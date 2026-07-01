namespace CareerPlatform.Domain.Entities;

public class UserCareerPath
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public int CareerPathId { get; set; }

    public User User { get; set; } = null!;

    public CareerPath CareerPath { get; set; } = null!;
}
