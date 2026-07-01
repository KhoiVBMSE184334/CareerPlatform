using CareerPlatform.Domain.Entities;
using CareerPlatform.Infrastructure.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerPlatform.Infrastructure.Configurations;

public class StudentProfileConfiguration : IEntityTypeConfiguration<StudentProfile>
{
    public void Configure(EntityTypeBuilder<StudentProfile> builder)
    {
        builder.ToTable("StudentProfiles");

        builder.HasKey(profile => profile.ProfileId);

        builder.Property(profile => profile.University)
            .HasMaxLength(150);

        builder.Property(profile => profile.Major)
            .HasMaxLength(100);

        builder.Property(profile => profile.GPA)
            .HasPrecision(3, 2);

        builder.Property(profile => profile.GithubUrl)
            .HasMaxLength(255);

        builder.HasIndex(profile => profile.UserId)
            .IsUnique();

        builder.HasOne(profile => profile.User)
            .WithOne(user => user.StudentProfile)
            .HasForeignKey<StudentProfile>(profile => profile.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(UserSeeder.StudentProfiles);
    }
}
