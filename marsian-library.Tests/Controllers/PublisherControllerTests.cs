using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using marsian_library.Controllers;
using marsian_library.Models;
using marsian_library.Services;

namespace marsian_library.Tests
{
    public class PublisherControllerTests
    {
        private readonly Mock<IPublisherService> _mockService;
        private readonly PublisherController _controller;

        public PublisherControllerTests()
        {
            _mockService = new Mock<IPublisherService>();
            _controller = new PublisherController(_mockService.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfPublishers()
        {
            // Arrange
            var mockPublishers = new List<Publisher> { new Publisher { Id = 1, Name = "Test" } };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(mockPublishers);

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Publisher>>(viewResult.ViewData.Model);
            Assert.Single(model);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenIdIsNull()
        {
            // Act
            var result = await _controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenPublisherDoesNotExist()
        {
            // Arrange
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((Publisher?)null);

            // Act
            var result = await _controller.Details(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithPublisher()
        {
            // Arrange
            var publisher = new Publisher { Id = 1, Name = "Test" };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(publisher);

            // Act
            var result = await _controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Publisher>(viewResult.ViewData.Model);
            Assert.Equal(1, model.Id);
        }

        [Fact]
        public async Task Create_Post_RedirectsToIndex_WhenModelStateIsValid()
        {
            // Arrange
            var publisher = new Publisher { Id = 1, Name = "New Publisher" };
            _mockService.Setup(s => s.CreateAsync(publisher)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(publisher);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            _mockService.Verify(s => s.CreateAsync(publisher), Times.Once);
        }

        [Fact]
        public async Task Create_Post_ReturnsViewWithPublisher_WhenModelStateIsInvalid()
        {
            // Arrange
            var publisher = new Publisher { Id = 1, Name = "" };
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Create(publisher);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(publisher, viewResult.Model);
            _mockService.Verify(s => s.CreateAsync(It.IsAny<Publisher>()), Times.Never);
        }

        [Fact]
        public async Task Edit_Post_ReturnsNotFound_WhenIdDoesNotMatchPublisherId()
        {
            // Arrange
            var publisher = new Publisher { Id = 1, Name = "Test" };

            // Act
            var result = await _controller.Edit(2, publisher);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteConfirmed_RedirectsToIndex_AfterDeleting()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteConfirmed(1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            _mockService.Verify(s => s.DeleteAsync(1), Times.Once);
        }
    }
}