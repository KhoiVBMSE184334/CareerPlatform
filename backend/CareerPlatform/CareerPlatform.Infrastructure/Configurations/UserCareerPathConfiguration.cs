using CareerPlatform.Domain.Entities;
using CareerPlatform.Infrastructure.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerPlatform.Infrastructure.Configurations;

public class UserCareerPathConfiguration : IEntityTypeConfiguration<UserCareerPath>
{
    public void Configure(EntityTypeBuilder<UserCareerPath> builder)
    {
        builder.ToTable("UserCareerPaths");

        builder.HasKey(userCareerPath => userCareerPath.Id);

        builder.HasIndex(userCareerPath => userCareerPath.UserId)
            .IsUnique();

        builder.HasIndex(userCareerPath => userCareerPath.CareerPathId);

        builder.HasOne(userCareerPath => userCareerPath.User)
            .WithMany(user => user.UserCareerPaths)
            .HasForeignKey(userCareerPath => userCareerPath.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(userCareerPath => userCareerPath.CareerPath)
            .WithMany(careerPath => careerPath.UserCareerPaths)
            .HasForeignKey(userCareerPath => userCareerPath.CareerPathId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(TestDataSeeder.UserCareerPaths);
    }
}
