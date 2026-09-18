using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Configurations;

public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.HasKey(task => task.Id);

        builder.Property(task => task.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(task => task.Description)
            .HasMaxLength(1000);

        builder.Property(task => task.IsCompleted)
            .IsRequired();

        builder.Property(task => task.DueDate)
            .IsRequired();

        builder.Property(task => task.CreatedAt)
            .IsRequired();
    }
}