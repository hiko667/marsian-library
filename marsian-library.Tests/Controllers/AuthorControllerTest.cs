using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using marsian_library.Controllers;
using marsian_library.Models;
using marsian_library.Services;
using Xunit;

namespace marsian_library.Tests.Controllers
{
    public class AuthorControllerTests
    {
        [Fact]
        public async Task Details_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            var mockService = new Mock<IAuthorService>();
            var controller = new AuthorController(mockService.Object);

            // Act
            var result = await controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithAuthor()
        {
            // Arrange
            var mockService = new Mock<IAuthorService>();
            mockService.Setup(s => s.GetAuthorByIdAsync(1))
            .ReturnsAsync(new Author { Id = 1, FirstName = "Jan", LastName = "Kowalski" });
            
            var controller = new AuthorController(mockService.Object);

            // Act
            var result = await controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Author>(viewResult.Model);
            Assert.Equal("Jan", model.FirstName);
        }
    }
}