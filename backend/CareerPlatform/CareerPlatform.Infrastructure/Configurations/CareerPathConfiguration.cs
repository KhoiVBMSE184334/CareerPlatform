using CareerPlatform.Domain.Entities;
using CareerPlatform.Infrastructure.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerPlatform.Infrastructure.Configurations;

public class CareerPathConfiguration : IEntityTypeConfiguration<CareerPath>
{
    public void Configure(EntityTypeBuilder<CareerPath> builder)
    {
        builder.ToTable("CareerPaths");

        builder.HasKey(careerPath => careerPath.CareerPathId);

        builder.Property(careerPath => careerPath.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(careerPath => careerPath.Description)
            .HasMaxLength(500);

        builder.HasIndex(careerPath => careerPath.Name)
            .IsUnique();

        builder.HasData(CareerPathSeeder.CareerPaths);
    }
}
