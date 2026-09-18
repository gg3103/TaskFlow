using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Interfaces;

public interface ITaskRepository
{
    Task<TaskItem> AddAsync(
        TaskItem task,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<TaskItem>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<TaskItem?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        TaskItem task,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        TaskItem task,
        CancellationToken cancellationToken = default);
}