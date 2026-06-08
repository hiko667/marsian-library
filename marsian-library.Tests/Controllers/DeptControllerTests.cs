using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using marsian_library.Controllers;
using marsian_library.Services;
using marsian_library.Models;

namespace marsian_library.Tests;

public class DeptControllerTests
{
    private readonly Mock<IDeptService> _mockService;
    private readonly DeptController _controller;

    public DeptControllerTests()
    {
        _mockService = new Mock<IDeptService>();
        _controller = new DeptController(_mockService.Object);
    }

    [Fact]
    public async Task Index_ReturnsAViewResult_WithAListOfDepartments()
    {
        // Arrange
        var mockDepts = new List<Dept> 
        { 
            new Dept 
            { 
                Id = 1, 
                AddressId = 10,
                Address = new Address { Id = 10, City = "Warszawa" },
                DirectorId = 5,
                Director = new Emp { Id = 5, FirstName = "Jan" }
            } 
        };
        _mockService.Setup(service => service.GetAllDeptsAsync()).ReturnsAsync(mockDepts);

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Dept>>(viewResult.ViewData.Model);
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
    public async Task Details_ReturnsNotFound_WhenDeptDoesNotExist()
    {
        // Arrange
        int testId = 1;
        _mockService.Setup(service => service.GetDeptByIdAsync(testId)).ReturnsAsync((Dept?)null);

        // Act
        var result = await _controller.Details(testId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Details_ReturnsViewResult_WithDepartment()
    {
        // Arrange
        int testId = 1;
        var mockDept = new Dept 
        { 
            Id = testId, 
            AddressId = 10,
            Address = new Address { Id = 10, City = "Warszawa" }
        };
        _mockService.Setup(service => service.GetDeptByIdAsync(testId)).ReturnsAsync(mockDept);

        // Act
        var result = await _controller.Details(testId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<Dept>(viewResult.ViewData.Model);
        Assert.Equal(testId, model.Id);
        Assert.Equal(10, model.AddressId);
    }
}