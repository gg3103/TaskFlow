using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskFlowDbContext _context;

        public TaskRepository(TaskFlowDbContext context)
        {
            _context = context;
        }

        public async Task<TaskItem> AddAsync(
            TaskItem task,
            CancellationToken cancellationToken = default)
        {
            await _context.Tasks.AddAsync(task, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return task;
        }

        public async Task<IReadOnlyCollection<TaskItem>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.Tasks
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<TaskItem?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await _context.Tasks
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(
            TaskItem task,
            CancellationToken cancellationToken = default)
        {
            _context.Tasks.Update(task);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(
            TaskItem task,
            CancellationToken cancellationToken = default)
        {
            _context.Tasks.Remove(task);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}