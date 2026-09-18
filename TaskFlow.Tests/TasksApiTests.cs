using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Tests
{
    public class TasksApiTests
    {
        [Fact]
        public async Task CreateTask_ShouldReturnCreated()
        {
            // Arrange
            await using var factory = new CustomWebApplicationFactory();

            using var client = factory.CreateClient();

            var request = new CreateTaskRequest
            {
                Title = "Integration Test Görevi",
                Description = "API integration test görevi",
                DueDate = new DateTime(2026, 12, 1, 18, 0, 0)
            };

            // Act
            var response = await client.PostAsJsonAsync(
                "/api/Tasks",
                request);

            // Assert
            Assert.Equal(
                HttpStatusCode.Created,
                response.StatusCode);
        }
        [Fact]
        public async Task GetTasks_ShouldReturnOk()
        {
            // Arrange
            await using var factory = new CustomWebApplicationFactory();

            using var client = factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/Tasks");

            // Assert
            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);
        }
        [Fact]
        public async Task GetTaskById_ShouldReturnOk_WhenTaskExists()
        {
            // Arrange
            await using var factory = new CustomWebApplicationFactory();

            using var client = factory.CreateClient();

            var createRequest = new CreateTaskRequest
            {
                Title = "GetById Integration Test",
                Description = "GetById integration test görevi",
                DueDate = new DateTime(2026, 12, 10, 18, 0, 0)
            };

            var createResponse = await client.PostAsJsonAsync(
                "/api/Tasks",
                createRequest);

            var createdTask =
                await createResponse.Content.ReadFromJsonAsync<TaskResponse>();

            // Act
            var response = await client.GetAsync(
                $"/api/Tasks/{createdTask!.Id}");

            // Assert
            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);
        }
        [Fact]
        public async Task GetTaskById_ShouldReturnNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            await using var factory = new CustomWebApplicationFactory();

            using var client = factory.CreateClient();

            var taskId = Guid.NewGuid();

            // Act
            var response = await client.GetAsync(
                $"/api/Tasks/{taskId}");

            // Assert
            Assert.Equal(
                HttpStatusCode.NotFound,
                response.StatusCode);
        }
        [Fact]
        public async Task UpdateTask_ShouldReturnOk_WhenTaskExists()
        {
            // Arrange
            await using var factory = new CustomWebApplicationFactory();

            using var client = factory.CreateClient();

            var createRequest = new CreateTaskRequest
            {
                Title = "Eski Başlık",
                Description = "Eski açıklama",
                DueDate = new DateTime(2026, 12, 10, 18, 0, 0)
            };

            var createResponse = await client.PostAsJsonAsync(
                "/api/Tasks",
                createRequest);

            var createdTask =
                await createResponse.Content.ReadFromJsonAsync<TaskResponse>();

            var updateRequest = new CreateTaskRequest
            {
                Title = "Güncellenmiş Başlık",
                Description = "Güncellenmiş açıklama",
                DueDate = new DateTime(2026, 12, 20, 20, 0, 0)
            };

            // Act
            var response = await client.PutAsJsonAsync(
                $"/api/Tasks/{createdTask!.Id}",
                updateRequest);

            // Assert
            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);

            var updatedTask =
                await response.Content.ReadFromJsonAsync<TaskResponse>();

            Assert.NotNull(updatedTask);
            Assert.Equal(
                "Güncellenmiş Başlık",
                updatedTask.Title);

            Assert.Equal(
                "Güncellenmiş açıklama",
                updatedTask.Description);

            Assert.Equal(
                new DateTime(2026, 12, 20, 20, 0, 0),
                updatedTask.DueDate);
        }
        [Fact]
        public async Task CompleteTask_ShouldReturnNoContent_AndCompleteTask()
        {
            // Arrange
            await using var factory = new CustomWebApplicationFactory();

            using var client = factory.CreateClient();

            var createRequest = new CreateTaskRequest
            {
                Title = "Complete Integration Test",
                Description = "Complete endpoint integration test",
                DueDate = new DateTime(2026, 12, 15, 18, 0, 0)
            };

            var createResponse = await client.PostAsJsonAsync(
                "/api/Tasks",
                createRequest);

            var createdTask =
                await createResponse.Content.ReadFromJsonAsync<TaskResponse>();

            Assert.NotNull(createdTask);
            Assert.False(createdTask.IsCompleted);

            // Act
            var completeResponse = await client.PutAsync(
                $"/api/Tasks/{createdTask.Id}/complete",
                null);

            // Assert
            Assert.Equal(
                HttpStatusCode.NoContent,
                completeResponse.StatusCode);

            var getResponse = await client.GetAsync(
                $"/api/Tasks/{createdTask.Id}");

            Assert.Equal(
                HttpStatusCode.OK,
                getResponse.StatusCode);

            var completedTask =
                await getResponse.Content.ReadFromJsonAsync<TaskResponse>();

            Assert.NotNull(completedTask);
            Assert.True(completedTask.IsCompleted);
        }
        [Fact]
        public async Task ReopenTask_ShouldReturnNoContent_AndReopenTask()
        {
            // Arrange
            await using var factory = new CustomWebApplicationFactory();

            using var client = factory.CreateClient();

            var createRequest = new CreateTaskRequest
            {
                Title = "Reopen Integration Test",
                Description = "Reopen endpoint integration test",
                DueDate = new DateTime(2026, 12, 20, 18, 0, 0)
            };

            var createResponse = await client.PostAsJsonAsync(
                "/api/Tasks",
                createRequest);

            var createdTask =
                await createResponse.Content.ReadFromJsonAsync<TaskResponse>();

            Assert.NotNull(createdTask);

            // Önce task'ı tamamla
            var completeResponse = await client.PutAsync(
                $"/api/Tasks/{createdTask.Id}/complete",
                null);

            Assert.Equal(
                HttpStatusCode.NoContent,
                completeResponse.StatusCode);

            // Task'ın tamamlandığını doğrula
            var completedGetResponse = await client.GetAsync(
                $"/api/Tasks/{createdTask.Id}");

            var completedTask =
                await completedGetResponse.Content.ReadFromJsonAsync<TaskResponse>();

            Assert.NotNull(completedTask);
            Assert.True(completedTask.IsCompleted);

            // Act
            var reopenResponse = await client.PutAsync(
                $"/api/Tasks/{createdTask.Id}/reopen",
                null);

            // Assert
            Assert.Equal(
                HttpStatusCode.NoContent,
                reopenResponse.StatusCode);

            var reopenedGetResponse = await client.GetAsync(
                $"/api/Tasks/{createdTask.Id}");

            Assert.Equal(
                HttpStatusCode.OK,
                reopenedGetResponse.StatusCode);

            var reopenedTask =
                await reopenedGetResponse.Content.ReadFromJsonAsync<TaskResponse>();

            Assert.NotNull(reopenedTask);
            Assert.False(reopenedTask.IsCompleted);
        }
        [Fact]
        public async Task DeleteTask_ShouldReturnNoContent_AndDeleteTask()
        {
            // Arrange
            await using var factory = new CustomWebApplicationFactory();

            using var client = factory.CreateClient();

            var createRequest = new CreateTaskRequest
            {
                Title = "Delete Integration Test",
                Description = "Delete endpoint integration test",
                DueDate = new DateTime(2026, 12, 25, 18, 0, 0)
            };

            var createResponse = await client.PostAsJsonAsync(
                "/api/Tasks",
                createRequest);

            var createdTask =
                await createResponse.Content.ReadFromJsonAsync<TaskResponse>();

            Assert.NotNull(createdTask);

            // Act
            var deleteResponse = await client.DeleteAsync(
                $"/api/Tasks/{createdTask.Id}");

            // Assert
            Assert.Equal(
                HttpStatusCode.NoContent,
                deleteResponse.StatusCode);

            var getResponse = await client.GetAsync(
                $"/api/Tasks/{createdTask.Id}");

            Assert.Equal(
                HttpStatusCode.NotFound,
                getResponse.StatusCode);
        }
        [Fact]
        public async Task DeleteTask_ShouldReturnNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            await using var factory = new CustomWebApplicationFactory();

            using var client = factory.CreateClient();

            var taskId = Guid.NewGuid();

            // Act
            var response = await client.DeleteAsync(
                $"/api/Tasks/{taskId}");

            // Assert
            Assert.Equal(
                HttpStatusCode.NotFound,
                response.StatusCode);

            var problemDetails =
                await response.Content.ReadFromJsonAsync<ProblemDetails>();

            Assert.NotNull(problemDetails);

            Assert.Equal(
                "Task not found.",
                problemDetails.Title);

            Assert.Equal(
                404,
                problemDetails.Status);
        }
    }
}