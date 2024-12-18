using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services;
using Core.Entities;
using Core.Interfaces;
using Moq;
using Xunit;

namespace TodoApi.Tests
{
    public class TodoServiceTests
    {
        private readonly TodoService _todoService;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ITodoRepository> _todoRepositoryMock;

        public TodoServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _todoRepositoryMock = new Mock<ITodoRepository>();

            _unitOfWorkMock.SetupGet(u => u.TodoRepository).Returns(_todoRepositoryMock.Object);
            _todoService = new TodoService(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetTodosAsync_ReturnsTodoItems()
        {
            // Arrange
            var todos = new List<TodoItem>
            {
                new TodoItem { Id = 1, Title = "Test Todo 1", Priority = 1 },
                new TodoItem { Id = 2, Title = "Test Todo 2", Priority = 2 }
            };

            _todoRepositoryMock.Setup(r => r.ListAsync()).ReturnsAsync(todos);

            // Act
            var result = await _todoService.GetTodosAsync();

            // Assert
            Assert.Equal(todos.Count, result.Count());
            Assert.Equal(todos[0].Title, result.First().Title);
        }

        [Fact]
        public async Task AddTodoAsync_AddsTodoItem()
        {
            // Arrange
            var todo = new TodoItem { Id = 1, Title = "Test Todo", Priority = 1 };

            // Act
            await _todoService.AddTodoAsync(todo);

            // Assert
            _todoRepositoryMock.Verify(r => r.AddAsync(todo), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task GetTodosByPriorityAsync_ReturnsFilteredTodoItems()
        {
            // Arrange
            var priority = 1;
            var todos = new List<TodoItem>
            {
                new TodoItem { Id = 1, Title = "Test Todo 1", Priority = priority },
                new TodoItem { Id = 2, Title = "Test Todo 2", Priority = priority }
            };

            _todoRepositoryMock.Setup(r => r.ListByPriorityAsync(priority)).ReturnsAsync(todos);

            // Act
            var result = await _todoService.GetTodosByPriorityAsync(priority);

            // Assert
            Assert.Equal(todos.Count, result.Count());
            Assert.All(result, t => Assert.Equal(priority, t.Priority));
        }

        // Additional Tests

        [Fact]
        public async Task GetTodosAsync_WhenNoTodos_ReturnsEmptyList()
        {
            // Arrange
            var todos = new List<TodoItem>();

            _todoRepositoryMock.Setup(r => r.ListAsync()).ReturnsAsync(todos);

            // Act
            var result = await _todoService.GetTodosAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task AddTodoAsync_WithNullTodo_ThrowsArgumentNullException()
        {
            // Arrange
            TodoItem todo = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _todoService.AddTodoAsync(todo));
        }

        [Fact]
        public async Task GetTodosByPriorityAsync_WithInvalidPriority_ReturnsEmptyList()
        {
            // Arrange
            var priority = -1; // Invalid priority
            var todos = new List<TodoItem>();

            _todoRepositoryMock.Setup(r => r.ListByPriorityAsync(priority)).ReturnsAsync(todos);

            // Act
            var result = await _todoService.GetTodosByPriorityAsync(priority);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task AddTodoAsync_WithDuplicateTodo_ThrowsException()
        {
            // Arrange
            var todo = new TodoItem { Id = 1, Title = "Test Todo", Priority = 1 };
            _todoRepositoryMock.Setup(r => r.AddAsync(todo)).Throws(new Exception("Duplicate todo"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _todoService.AddTodoAsync(todo));
            Assert.Equal("Duplicate todo", exception.Message);
        }

        [Fact]
        public async Task GetTodosByPriorityAsync_WithNoMatchingTodos_ReturnsEmptyList()
        {
            // Arrange
            var priority = 3;
            var todos = new List<TodoItem>();

            _todoRepositoryMock.Setup(r => r.ListByPriorityAsync(priority)).ReturnsAsync(todos);

            // Act
            var result = await _todoService.GetTodosByPriorityAsync(priority);

            // Assert
            Assert.Empty(result);
        }

        // Advanced Test Cases

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetTodosByPriorityAsync_WithVariousPriorities_ReturnsCorrectTodos(int priority)
        {
            // Arrange
            var todos = new List<TodoItem>
            {
                new TodoItem { Id = 1, Title = "Test Todo 1", Priority = priority },
                new TodoItem { Id = 2, Title = "Test Todo 2", Priority = priority }
            };

            _todoRepositoryMock.Setup(r => r.ListByPriorityAsync(priority)).ReturnsAsync(todos);

            // Act
            var result = await _todoService.GetTodosByPriorityAsync(priority);

            // Assert
            Assert.All(result, t => Assert.Equal(priority, t.Priority));
        }

        [Fact]
        public async Task AddTodoAsync_WithInvalidData_ThrowsValidationException()
        {
            // Arrange
            var todo = new TodoItem { Id = 1, Title = "", Priority = -1 };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _todoService.AddTodoAsync(todo));
            Assert.Contains("Title is required.", exception.Message);
            Assert.Contains("Priority must be between 1 and 5.", exception.Message);
        }

        // Exception Handling Test

        [Fact]
        public async Task GetTodosAsync_WhenRepositoryThrowsException_LogsErrorAndRethrows()
        {
            // Arrange
            var expectedMessage = "Test exception";
            _todoRepositoryMock.Setup(r => r.ListAsync()).ThrowsAsync(new Exception(expectedMessage));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _todoService.GetTodosAsync());
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public async Task AddTodoAsync_WhenUnitOfWorkThrowsException_LogsErrorAndRethrows()
        {
            // Arrange
            var todo = new TodoItem { Id = 1, Title = "Test Todo", Priority = 1 };
            var expectedMessage = "Test exception";
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ThrowsAsync(new Exception(expectedMessage));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _todoService.AddTodoAsync(todo));
            Assert.Equal(expectedMessage, exception.Message);
        }

        // Edge Case Test

        [Fact]
        public async Task GetTodosByPriorityAsync_WithMaximumPriority_ReturnsCorrectTodos()
        {
            // Arrange
            var priority = 5; // Maximum priority
            var todos = new List<TodoItem>
            {
                new TodoItem { Id = 1, Title = "Test Todo 1", Priority = priority },
                new TodoItem { Id = 2, Title = "Test Todo 2", Priority = priority }
            };

            _todoRepositoryMock.Setup(r => r.ListByPriorityAsync(priority)).ReturnsAsync(todos);

            // Act
            var result = await _todoService.GetTodosByPriorityAsync(priority);

            // Assert
            Assert.All(result, t => Assert.Equal(priority, t.Priority));
        }
    }
}
