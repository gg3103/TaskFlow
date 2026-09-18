using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Data;

public class TaskFlowDbContext : DbContext
{
    public TaskFlowDbContext(
        DbContextOptions<TaskFlowDbContext> options)
        : base(options)
    {
    }

    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(TaskFlowDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}