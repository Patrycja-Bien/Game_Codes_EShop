using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Game_Codes_EShop_Domain.Models;
using Game_Codes_EShopService.Controllers;
using Microsoft.AspNetCore.Mvc;
using Game_Codes_EShop_Application.Services;

namespace Game_Codes_EShopService_Tests.Controllers;

public class Game_Controller_Tests
{
    private readonly Mock<IGame_Service> _mockService;
    private readonly GameController _controller;

    public Game_Controller_Tests()
    {
        _mockService = new Mock<IGame_Service>();
        _controller = new GameController(_mockService.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnAllGames_ReturnTrue()
    {
        // Arrange
        var products = new List<Game> { new Game(), new Game() };
        _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(products);

        // Act
        var result = await _controller.GetAsync();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(products, okResult.Value);
    }

    [Fact]
    public async Task Get_WithValidId_ReturnsGame_ReturnTrue()
    {
        // Arrange
        var product = new Game { Id = 1 };
        _mockService.Setup(s => s.GetAsync(1)).ReturnsAsync(product);

        // Act
        var result = await _controller.GetAsync(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(product, okResult.Value);
    }

    [Fact]
    public async Task Get_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        _mockService.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync((Game)null);

        // Act
        var result = await _controller.GetAsync(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Post_ValidProduct_ReturnsCreatedProduct()
    {
        // Arrange
        var newProduct = new Game();
        _mockService.Setup(s => s.Add(It.IsAny<Game>())).ReturnsAsync(newProduct);

        // Act
        var result = await _controller.PostAsync(newProduct);

        // Assert
        _mockService.Verify(s => s.Add(newProduct), Times.Once);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Put_ValidProduct_UpdatesAndReturnsOk()
    {
        // Arrange
        var product = new Game { Id = 1 };
        _mockService.Setup(s => s.UpdateAsync(product)).ReturnsAsync(product);

        // Act
        var result = await _controller.PutAsync(1, product);

        // Assert
        _mockService.Verify(s => s.UpdateAsync(product), Times.Once);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Delete_ValidId_MarksDeletedAndUpdates()
    {
        // Arrange
        var product = new Game { Id = 1, Deleted = false };
        _mockService.Setup(s => s.GetAsync(1)).ReturnsAsync(product);
        _mockService.Setup(s => s.UpdateAsync(It.IsAny<Game>())).ReturnsAsync(product);

        // Act
        var result = await _controller.DeleteAsync(1);

        // Assert
        _mockService.Verify(s => s.GetAsync(1), Times.Once);
        _mockService.Verify(s => s.UpdateAsync(It.Is<Game>(p => p.Deleted)), Times.Once);
        Assert.IsType<OkObjectResult>(result);
    }
}
