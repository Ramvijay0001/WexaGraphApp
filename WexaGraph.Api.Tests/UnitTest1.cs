using Microsoft.AspNetCore.Mvc;
using Moq;
using Neo4j.Driver;
using WexaGraph.Api.Controllers;
using WexaGraph.Api.Services;

namespace WexaGraph.Api.Tests;

public class DatabaseControllerTests
{
    [Fact]
    public async Task TestConnection_ShouldReturnOk_WhenDatabaseConnected()
    {
        // Arrange
        var cognoDbService = new Mock<ICognoDbService>();
        var driver = new Mock<IDriver>();

        var seedService = new SeedService(driver.Object);

        cognoDbService
            .Setup(x => x.TestConnectionAsync())
            .ReturnsAsync(true);

        var controller = new DatabaseController(
            cognoDbService.Object,
            seedService);

        // Act
        var result = await controller.TestConnection();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.NotNull(okResult.Value);
    }
    [Fact]
    public async Task GetProjectsByTechnology_ShouldReturnProjects()
    {
        // Arrange
        var cognoDbService = new Mock<ICognoDbService>();
        var driver = new Mock<IDriver>();
        var seedService = new SeedService(driver.Object);

        cognoDbService
            .Setup(x => x.GetProjectsByTechnologyAsync("Angular"))
            .ReturnsAsync(new List<string>
            {
            "Banking API Platform",
            "Healthcare Portal"
            });

        var controller = new DatabaseController(
            cognoDbService.Object,
            seedService);

        // Act
        var result = await controller.GetProjectsByTechnology("Angular");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.NotNull(okResult.Value);
    }
}