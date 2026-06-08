using System.Collections.Generic;
using System.Threading.Tasks;
using marsian_library.Controllers;
using marsian_library.Models;
using marsian_library.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace marsian_library.Tests
{
    public class LanguageControllerTests
    {
        private readonly Mock<ILanguageService> _mockService;
        private readonly LanguageController _controller;

        public LanguageControllerTests()
        {
            _mockService = new Mock<ILanguageService>();
            _controller = new LanguageController(_mockService.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfLanguages()
        {
            // Arrange
            var mockLanguages = new List<Language>
            {
                new Language { Id = 1, Name = "English" },
                new Language { Id = 2, Name = "Polish" }
            };
            _mockService.Setup(service => service.GetAllAsync()).ReturnsAsync(mockLanguages);

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Language>>(viewResult.ViewData.Model);
            Assert.Equal(2, ((List<Language>)model).Count);
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
        public async Task Details_ReturnsNotFound_WhenLanguageDoesNotExist()
        {
            // Arrange
            int testId = 1;
            _mockService.Setup(service => service.GetByIdAsync(testId)).ReturnsAsync((Language)null);

            // Act
            var result = await _controller.Details(testId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithLanguage()
        {
            // Arrange
            int testId = 1;
            var mockLanguage = new Language { Id = testId, Name = "English" };
            _mockService.Setup(service => service.GetByIdAsync(testId)).ReturnsAsync(mockLanguage);

            // Act
            var result = await _controller.Details(testId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Language>(viewResult.ViewData.Model);
            Assert.Equal(testId, model.Id);
        }
    }
}