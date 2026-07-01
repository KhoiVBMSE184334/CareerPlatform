using CareerPlatform.Domain.Entities;
using CareerPlatform.Infrastructure.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerPlatform.Infrastructure.Configurations;

public class PortfolioProjectConfiguration : IEntityTypeConfiguration<PortfolioProject>
{
    public void Configure(EntityTypeBuilder<PortfolioProject> builder)
    {
        builder.ToTable("PortfolioProjects");

        builder.HasKey(project => project.ProjectId);

        builder.Property(project => project.RepositoryName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(project => project.GithubUrl)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(project => project.ImportedAt)
            
            .IsRequired();

        builder.HasIndex(project => project.UserId);

        builder.HasIndex(project => new { project.UserId, project.GithubUrl })
            .IsUnique();

        builder.HasOne(project => project.User)
            .WithMany(user => user.PortfolioProjects)
            .HasForeignKey(project => project.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(TestDataSeeder.PortfolioProjects);
    }
}
