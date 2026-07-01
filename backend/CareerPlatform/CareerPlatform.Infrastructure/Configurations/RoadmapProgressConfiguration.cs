using CareerPlatform.Domain.Entities;
using CareerPlatform.Infrastructure.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerPlatform.Infrastructure.Configurations;

public class RoadmapProgressConfiguration : IEntityTypeConfiguration<RoadmapProgress>
{
    public void Configure(EntityTypeBuilder<RoadmapProgress> builder)
    {
        builder.ToTable("RoadmapProgresses");

        builder.HasKey(progress => progress.ProgressId);

        builder.Property(progress => progress.IsCompleted)
            .IsRequired();

        builder.Property(progress => progress.CompletedAt)
            ;

        builder.HasIndex(progress => progress.UserId);

        builder.HasIndex(progress => progress.SkillNodeId);

        builder.HasIndex(progress => new { progress.UserId, progress.SkillNodeId })
            .IsUnique();

        builder.HasOne(progress => progress.User)
            .WithMany(user => user.RoadmapProgresses)
            .HasForeignKey(progress => progress.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(progress => progress.SkillNode)
            .WithMany(skillNode => skillNode.RoadmapProgresses)
            .HasForeignKey(progress => progress.SkillNodeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(TestDataSeeder.RoadmapProgresses);
    }
}
