namespace TaskFlow.Application.DTOs;

public sealed class TaskResponse
{
    public Guid Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public string? Description { get; init; }

    public bool IsCompleted { get; init; }

    public DateTime? DueDate { get; init; }

    public DateTime CreatedAt { get; init; }
}