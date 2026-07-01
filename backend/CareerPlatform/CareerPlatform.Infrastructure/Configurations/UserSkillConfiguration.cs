using CareerPlatform.Domain.Entities;
using CareerPlatform.Infrastructure.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerPlatform.Infrastructure.Configurations;

public class UserSkillConfiguration : IEntityTypeConfiguration<UserSkill>
{
    public void Configure(EntityTypeBuilder<UserSkill> builder)
    {
        builder.ToTable("UserSkills");

        builder.HasKey(userSkill => userSkill.UserSkillId);

        builder.Property(userSkill => userSkill.SkillName)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(userSkill => userSkill.UserId);

        builder.HasIndex(userSkill => new { userSkill.UserId, userSkill.SkillName })
            .IsUnique();

        builder.HasOne(userSkill => userSkill.User)
            .WithMany(user => user.UserSkills)
            .HasForeignKey(userSkill => userSkill.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(TestDataSeeder.UserSkills);
    }
}
