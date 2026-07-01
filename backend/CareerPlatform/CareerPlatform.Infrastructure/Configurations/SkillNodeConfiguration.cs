using CareerPlatform.Domain.Entities;
using CareerPlatform.Domain.Enums;
using CareerPlatform.Infrastructure.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerPlatform.Infrastructure.Configurations;

public class SkillNodeConfiguration : IEntityTypeConfiguration<SkillNode>
{
    public void Configure(EntityTypeBuilder<SkillNode> builder)
    {
        builder.ToTable("SkillNodes");

        builder.HasKey(skillNode => skillNode.SkillNodeId);

        builder.Property(skillNode => skillNode.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(skillNode => skillNode.Description)
            .HasMaxLength(500);

        builder.Property(skillNode => skillNode.Difficulty)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(skillNode => skillNode.DisplayOrder)
            .IsRequired();

        builder.HasIndex(skillNode => skillNode.CareerPathId);

        builder.HasIndex(skillNode => new { skillNode.CareerPathId, skillNode.DisplayOrder })
            .IsUnique();

        builder.HasOne(skillNode => skillNode.CareerPath)
            .WithMany(careerPath => careerPath.SkillNodes)
            .HasForeignKey(skillNode => skillNode.CareerPathId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(SkillNodeSeeder.SkillNodes);
    }
}
