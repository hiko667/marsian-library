using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using marsian_library.Controllers;
using marsian_library.Services;
using Xunit;

namespace marsian_library.Tests
{
    public class BorrowControllerTests
    {
        [Fact]
        public async Task Return_ReturnsNotFound_WhenBorrowDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IBorrowService>();
            
            mockService.Setup(s => s.ReturnBookAsync(999)).ReturnsAsync(false);
            
            var controller = new BorrowController(mockService.Object);

            // Act
            var result = await controller.Return(999, null);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }
    }
}