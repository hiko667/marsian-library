using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;
using marsian_library.Controllers;
using marsian_library.Services;
using marsian_library.Models;

namespace marsian_library.Tests;

public class BookApiControllerTests
{
    private readonly Mock<IBookService> _mockBookService;
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly BookApiController _controller;

    public BookApiControllerTests()
    {
        _mockBookService = new Mock<IBookService>();
        
        // Mock UserManager
        _mockUserManager = new Mock<UserManager<ApplicationUser>>(
            new Mock<IUserStore<ApplicationUser>>().Object,
            null, null, null, null, null, null, null, null
        );
        
        _controller = new BookApiController(_mockBookService.Object, _mockUserManager.Object);
    }

    [Fact]
    public async Task GetAllBooks_ReturnsOkResult_WithListOfBooks()
    {
        // Arrange
        var mockBooks = new List<object> { new { Guid = "123", Title = "Test Book" } };
        _mockBookService.Setup(s => s.GetAllBooksAsync()).ReturnsAsync(mockBooks);

        // Act
        var result = await _controller.GetAllBooks();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(mockBooks, okResult.Value);
    }

    [Fact]
    public async Task GetBook_ReturnsOkResult_WhenBookExists()
    {
        // Arrange
        var guid = "existing-guid";
        var mockBook = new { Guid = guid, Title = "Test Book" };
        _mockBookService.Setup(s => s.GetBookByGuidAsync(guid)).ReturnsAsync(mockBook);

        // Act
        var result = await _controller.GetBook(guid);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(mockBook, okResult.Value);
    }

    [Fact]
    public async Task GetBook_ReturnsNotFoundResult_WhenBookDoesNotExist()
    {
        // Arrange
        var guid = "non-existing-guid";
        _mockBookService.Setup(s => s.GetBookByGuidAsync(guid)).ReturnsAsync((object?)null);

        // Act
        var result = await _controller.GetBook(guid);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetAvailable_ReturnsOkResult_WithAvailableBooks()
    {
        // Arrange
        var mockBooks = new List<object> { new { Guid = "456", Title = "Available Book" } };
        _mockBookService.Setup(s => s.GetAvailableBooksAsync()).ReturnsAsync(mockBooks);

        // Act
        var result = await _controller.GetAvailable();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(mockBooks, okResult.Value);
    }
}