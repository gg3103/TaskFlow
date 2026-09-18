using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Interfaces
{
    public interface ITaskService
    {
        Task<TaskResponse> CreateAsync(
            CreateTaskRequest request,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<TaskResponse>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<TaskResponse?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<TaskResponse?> UpdateAsync(
            Guid id,
            CreateTaskRequest request,
            CancellationToken cancellationToken = default);

        Task CompleteAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task ReopenAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            Guid id,
            CancellationToken cancellationToken = default);
    }
}