using CareerPlatform.Domain.Entities;
using CareerPlatform.Infrastructure.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerPlatform.Infrastructure.Configurations;

public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.ToTable("ChatMessages");

        builder.HasKey(message => message.MessageId);

        builder.Property(message => message.Role)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(message => message.Content)
            .IsRequired();

        builder.Property(message => message.CreatedAt)
            
            .IsRequired();

        builder.HasIndex(message => message.SessionId);

        builder.HasIndex(message => new { message.SessionId, message.CreatedAt });

        builder.HasOne(message => message.ChatSession)
            .WithMany(session => session.ChatMessages)
            .HasForeignKey(message => message.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(TestDataSeeder.ChatMessages);
    }
}
