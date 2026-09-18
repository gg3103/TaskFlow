namespace TaskFlow.Domain.Entities;

public class TaskItem
{
    public Guid Id { get; private set; }

    public string Title { get; private set; }

    public string? Description { get; private set; }

    public bool IsCompleted { get; private set; }

    public DateTime? DueDate { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public TaskItem(
        string title,
        string? description = null,
        DateTime? dueDate = null)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException(
                "Task title cannot be empty.",
                nameof(title));
        }

        Id = Guid.NewGuid();
        Title = title.Trim();
        Description = description?.Trim();
        DueDate = dueDate;
        CreatedAt = DateTime.UtcNow;
        IsCompleted = false;
    }

    public void Complete()
    {
        IsCompleted = true;
    }

    public void Reopen()
    {
        IsCompleted = false;
    }

    public void Update(
        string title,
        string? description,
        DateTime? dueDate)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException(
                "Task title cannot be empty.",
                nameof(title));
        }

        Title = title.Trim();
        Description = description?.Trim();
        DueDate = dueDate;
    }
}