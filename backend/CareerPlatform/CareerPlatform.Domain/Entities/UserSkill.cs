namespace CareerPlatform.Domain.Entities;

public class UserSkill
{
    public int UserSkillId { get; set; }

    public Guid UserId { get; set; }

    public string SkillName { get; set; } = string.Empty;

    public User User { get; set; } = null!;
}
