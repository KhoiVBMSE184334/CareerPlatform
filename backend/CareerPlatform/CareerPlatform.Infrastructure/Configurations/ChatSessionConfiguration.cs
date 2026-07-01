using CareerPlatform.Domain.Entities;
using CareerPlatform.Infrastructure.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerPlatform.Infrastructure.Configurations;

public class ChatSessionConfiguration : IEntityTypeConfiguration<ChatSession>
{
    public void Configure(EntityTypeBuilder<ChatSession> builder)
    {
        builder.ToTable("ChatSessions");

        builder.HasKey(session => session.SessionId);

        builder.Property(session => session.CreatedAt)
            
            .IsRequired();

        builder.HasIndex(session => session.UserId);

        builder.HasOne(session => session.User)
            .WithMany(user => user.ChatSessions)
            .HasForeignKey(session => session.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(TestDataSeeder.ChatSessions);
    }
}
