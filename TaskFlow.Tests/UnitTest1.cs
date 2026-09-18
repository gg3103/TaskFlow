using Moq;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Application.Services;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests
{
    public class TaskServiceTests
    {
        [Fact]
        public async Task CreateAsync_ShouldCreateTask()
        {
            // Arrange
            var repositoryMock = new Mock<ITaskRepository>();

            var request = new CreateTaskRequest
            {
                Title = "Test Görevi",
                Description = "Unit test görevi",
                DueDate = new DateTime(2026, 9, 30, 18, 0, 0)
            };

            var createdTask = new TaskItem(
                request.Title,
                request.Description,
                request.DueDate);

            repositoryMock
                .Setup(repository => repository.AddAsync(
                    It.IsAny<TaskItem>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdTask);

            var service = new TaskService(
                repositoryMock.Object);

            // Act
            var result = await service.CreateAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.Title, result.Title);
            Assert.Equal(request.Description, result.Description);
            Assert.Equal(request.DueDate, result.DueDate);

            repositoryMock.Verify(
                repository => repository.AddAsync(
                    It.IsAny<TaskItem>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnTask()
        {
            // Arrange
            var repositoryMock = new Mock<ITaskRepository>();

            var taskId = Guid.NewGuid();

            var task = new TaskItem(
                "Get Test Görevi",
                "GetById unit test görevi",
                new DateTime(2026, 10, 1, 18, 0, 0));

            repositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    taskId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(task);

            var service = new TaskService(
                repositoryMock.Object);

            // Act
            var result = await service.GetByIdAsync(taskId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(task.Title, result.Title);
            Assert.Equal(task.Description, result.Description);
            Assert.Equal(task.DueDate, result.DueDate);

            repositoryMock.Verify(
                repository => repository.GetByIdAsync(
                    taskId,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateTask()
        {
            // Arrange
            var repositoryMock = new Mock<ITaskRepository>();

            var taskId = Guid.NewGuid();

            var task = new TaskItem(
                "Eski Baþlýk",
                "Eski açýklama",
                new DateTime(2026, 9, 20, 18, 0, 0));

            var request = new CreateTaskRequest
            {
                Title = "Yeni Baþlýk",
                Description = "Yeni açýklama",
                DueDate = new DateTime(2026, 10, 5, 18, 0, 0)
            };

            repositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    taskId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(task);

            var service = new TaskService(
                repositoryMock.Object);

            // Act
            var result = await service.UpdateAsync(
                taskId,
                request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Yeni Baþlýk", result.Title);
            Assert.Equal("Yeni açýklama", result.Description);
            Assert.Equal(
                new DateTime(2026, 10, 5, 18, 0, 0),
                result.DueDate);

            repositoryMock.Verify(
                repository => repository.GetByIdAsync(
                    taskId,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            repositoryMock.Verify(
                repository => repository.UpdateAsync(
                    It.IsAny<TaskItem>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task CompleteAsync_ShouldCompleteTask()
        {
            // Arrange
            var repositoryMock = new Mock<ITaskRepository>();

            var taskId = Guid.NewGuid();

            var task = new TaskItem(
                "Complete Test Görevi",
                "Complete unit test görevi",
                new DateTime(2026, 10, 10, 18, 0, 0));

            repositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    taskId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(task);

            var service = new TaskService(
                repositoryMock.Object);

            // Act
            await service.CompleteAsync(taskId);

            // Assert
            Assert.True(task.IsCompleted);

            repositoryMock.Verify(
                repository => repository.GetByIdAsync(
                    taskId,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            repositoryMock.Verify(
                repository => repository.UpdateAsync(
                    It.IsAny<TaskItem>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task ReopenAsync_ShouldReopenTask()
        {
            // Arrange
            var repositoryMock = new Mock<ITaskRepository>();

            var taskId = Guid.NewGuid();

            var task = new TaskItem(
                "Reopen Test Görevi",
                "Reopen unit test görevi",
                new DateTime(2026, 10, 15, 18, 0, 0));

            task.Complete();

            repositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    taskId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(task);

            var service = new TaskService(
                repositoryMock.Object);

            // Act
            await service.ReopenAsync(taskId);

            // Assert
            Assert.False(task.IsCompleted);

            repositoryMock.Verify(
                repository => repository.GetByIdAsync(
                    taskId,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            repositoryMock.Verify(
                repository => repository.UpdateAsync(
                    It.IsAny<TaskItem>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteTask()
        {
            // Arrange
            var repositoryMock = new Mock<ITaskRepository>();

            var taskId = Guid.NewGuid();

            var task = new TaskItem(
                "Delete Test Görevi",
                "Delete unit test görevi",
                new DateTime(2026, 10, 20, 18, 0, 0));

            repositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    taskId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(task);

            var service = new TaskService(
                repositoryMock.Object);

            // Act
            await service.DeleteAsync(taskId);

            // Assert
            repositoryMock.Verify(
                repository => repository.GetByIdAsync(
                    taskId,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            repositoryMock.Verify(
                repository => repository.DeleteAsync(
                    task,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenTaskDoesNotExist()
        {
            // Arrange
            var repositoryMock = new Mock<ITaskRepository>();

            var taskId = Guid.NewGuid();

            repositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    taskId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((TaskItem?)null);

            var service = new TaskService(
                repositoryMock.Object);

            // Act
            var result = await service.GetByIdAsync(taskId);

            // Assert
            Assert.Null(result);

            repositoryMock.Verify(
                repository => repository.GetByIdAsync(
                    taskId,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task CompleteAsync_ShouldThrowKeyNotFoundException_WhenTaskDoesNotExist()
        {
            // Arrange
            var repositoryMock = new Mock<ITaskRepository>();

            var taskId = Guid.NewGuid();

            repositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    taskId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((TaskItem?)null);

            var service = new TaskService(
                repositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => service.CompleteAsync(taskId));

            repositoryMock.Verify(
                repository => repository.GetByIdAsync(
                    taskId,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            repositoryMock.Verify(
                repository => repository.UpdateAsync(
                    It.IsAny<TaskItem>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task ReopenAsync_ShouldThrowKeyNotFoundException_WhenTaskDoesNotExist()
        {
            // Arrange
            var repositoryMock = new Mock<ITaskRepository>();

            var taskId = Guid.NewGuid();

            repositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    taskId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((TaskItem?)null);

            var service = new TaskService(
                repositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => service.ReopenAsync(taskId));

            repositoryMock.Verify(
                repository => repository.GetByIdAsync(
                    taskId,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            repositoryMock.Verify(
                repository => repository.UpdateAsync(
                    It.IsAny<TaskItem>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowKeyNotFoundException_WhenTaskDoesNotExist()
        {
            // Arrange
            var repositoryMock = new Mock<ITaskRepository>();

            var taskId = Guid.NewGuid();

            repositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    taskId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((TaskItem?)null);

            var service = new TaskService(
                repositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => service.DeleteAsync(taskId));

            repositoryMock.Verify(
                repository => repository.GetByIdAsync(
                    taskId,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            repositoryMock.Verify(
                repository => repository.DeleteAsync(
                    It.IsAny<TaskItem>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }

    public class TaskItemTests
    {
        [Fact]
        public void Complete_ShouldSetIsCompletedToTrue()
        {
            // Arrange
            var task = new TaskItem(
                "Test Görevi",
                "Test açýklamasý",
                new DateTime(2026, 10, 1, 18, 0, 0));

            // Act
            task.Complete();

            // Assert
            Assert.True(task.IsCompleted);
        }
        [Fact]
        public void Reopen_ShouldSetIsCompletedToFalse()
        {
            // Arrange
            var task = new TaskItem(
                "Test Görevi",
                "Test açýklamasý",
                new DateTime(2026, 10, 1, 18, 0, 0));

            task.Complete();

            // Act
            task.Reopen();

            // Assert
            Assert.False(task.IsCompleted);
        }
        [Fact]
        public void Update_ShouldUpdateTaskProperties()
        {
            // Arrange
            var task = new TaskItem(
                "Eski Baþlýk",
                "Eski açýklama",
                new DateTime(2026, 10, 1, 18, 0, 0));

            var newTitle = "Yeni Baþlýk";
            var newDescription = "Yeni açýklama";
            var newDueDate = new DateTime(2026, 11, 15, 20, 0, 0);

            // Act
            task.Update(
                newTitle,
                newDescription,
                newDueDate);

            // Assert
            Assert.Equal(newTitle, task.Title);
            Assert.Equal(newDescription, task.Description);
            Assert.Equal(newDueDate, task.DueDate);
        }
    }
}