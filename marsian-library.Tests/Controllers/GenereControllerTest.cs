using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using marsian_library.Controllers;
using marsian_library.Models;
using marsian_library.Services;
using Xunit;

namespace marsian_library.Tests
{
    public class GenreControllerTests
    {
        [Fact]
        public async Task Details_IdValid_ReturnsViewWithGenreSir()
        {
            // Arrange
            var mockService = new Mock<IGenreService>();
            var expectedGenre = new Genre { Id = 1, Name = "Ninja Novel"};
            
            mockService.Setup(service => service.GetByIdAsync(1))
                       .ReturnsAsync(expectedGenre);

            var controller = new GenreController(mockService.Object);

            // Act
            var result = await controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Genre>(viewResult.Model);
            Assert.Equal(1, model.Id);
            Assert.Equal("Ninja Novel", model.Name);
        }
    }
}