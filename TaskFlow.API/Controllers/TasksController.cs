using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<ActionResult<TaskResponse>> Create(
            CreateTaskRequest request,
            CancellationToken cancellationToken)
        {
            var task = await _taskService.CreateAsync(
                request,
                cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = task.Id },
                task);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<TaskResponse>>> GetAll(
            CancellationToken cancellationToken)
        {
            var tasks = await _taskService.GetAllAsync(
                cancellationToken);

            return Ok(tasks);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TaskResponse>> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var task = await _taskService.GetByIdAsync(
                id,
                cancellationToken);

            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<TaskResponse>> Update(
            Guid id,
            CreateTaskRequest request,
            CancellationToken cancellationToken)
        {
            var task = await _taskService.UpdateAsync(
                id,
                request,
                cancellationToken);

            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpPut("{id:guid}/complete")]
        public async Task<IActionResult> Complete(
            Guid id,
            CancellationToken cancellationToken)
        {
            await _taskService.CompleteAsync(
                id,
                cancellationToken);

            return NoContent();
        }

        [HttpPut("{id:guid}/reopen")]
        public async Task<IActionResult> Reopen(
            Guid id,
            CancellationToken cancellationToken)
        {
            await _taskService.ReopenAsync(
                id,
                cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            await _taskService.DeleteAsync(
                id,
                cancellationToken);

            return NoContent();
        }
    }
}