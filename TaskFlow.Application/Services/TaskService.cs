using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<TaskResponse> CreateAsync(
            CreateTaskRequest request,
            CancellationToken cancellationToken = default)
        {
            var task = new TaskItem(
                request.Title,
                request.Description,
                request.DueDate);

            var createdTask = await _taskRepository.AddAsync(
                task,
                cancellationToken);

            return MapToResponse(createdTask);
        }

        public async Task<IReadOnlyCollection<TaskResponse>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var tasks = await _taskRepository.GetAllAsync(
                cancellationToken);

            return tasks
                .Select(MapToResponse)
                .ToList();
        }

        public async Task<TaskResponse?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var task = await _taskRepository.GetByIdAsync(
                id,
                cancellationToken);

            if (task == null)
                return null;

            return MapToResponse(task);
        }

        public async Task<TaskResponse?> UpdateAsync(
            Guid id,
            CreateTaskRequest request,
            CancellationToken cancellationToken = default)
        {
            var task = await _taskRepository.GetByIdAsync(
                id,
                cancellationToken);

            if (task == null)
                return null;

            task.Update(
                request.Title,
                request.Description,
                request.DueDate);

            await _taskRepository.UpdateAsync(
                task,
                cancellationToken);

            return MapToResponse(task);
        }

        public async Task CompleteAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var task = await _taskRepository.GetByIdAsync(
                id,
                cancellationToken);

            if (task == null)
                throw new KeyNotFoundException("Task not found.");

            task.Complete();

            await _taskRepository.UpdateAsync(
                task,
                cancellationToken);
        }

        public async Task ReopenAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var task = await _taskRepository.GetByIdAsync(
                id,
                cancellationToken);

            if (task == null)
                throw new KeyNotFoundException("Task not found.");

            task.Reopen();

            await _taskRepository.UpdateAsync(
                task,
                cancellationToken);
        }

        public async Task DeleteAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var task = await _taskRepository.GetByIdAsync(
                id,
                cancellationToken);

            if (task == null)
                throw new KeyNotFoundException("Task not found.");

            await _taskRepository.DeleteAsync(
                task,
                cancellationToken);
        }

        private static TaskResponse MapToResponse(TaskItem task)
        {
            return new TaskResponse
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                DueDate = task.DueDate,
                CreatedAt = task.CreatedAt
            };
        }
    }
}