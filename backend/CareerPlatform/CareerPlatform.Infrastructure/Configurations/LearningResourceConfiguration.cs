using CareerPlatform.Domain.Entities;
using CareerPlatform.Infrastructure.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerPlatform.Infrastructure.Configurations;

public class LearningResourceConfiguration : IEntityTypeConfiguration<LearningResource>
{
    public void Configure(EntityTypeBuilder<LearningResource> builder)
    {
        builder.ToTable("LearningResources");

        builder.HasKey(resource => resource.ResourceId);

        builder.Property(resource => resource.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(resource => resource.Url)
            .HasMaxLength(500)
            .IsRequired();

        builder.HasIndex(resource => resource.SkillNodeId);

        builder.HasOne(resource => resource.SkillNode)
            .WithMany(skillNode => skillNode.LearningResources)
            .HasForeignKey(resource => resource.SkillNodeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(LearningResourceSeeder.LearningResources);
    }
}
