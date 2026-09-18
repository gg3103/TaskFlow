using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Application.DTOs
{
    public class CreateTaskRequest
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(
            200,
            MinimumLength = 3,
            ErrorMessage = "Title must be between 3 and 200 characters.")]
        public string Title { get; set; } = string.Empty;

        [StringLength(
            1000,
            ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "DueDate is required.")]
        public DateTime DueDate { get; set; }
    }
}