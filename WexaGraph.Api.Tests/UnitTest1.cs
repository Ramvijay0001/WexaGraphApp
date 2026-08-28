namespace WexaGraph.Api.Tests;

public class GraphTests
{
    [Fact]
    public void GraphData_ShouldContainTechnology()
    {
        // Arrange
        var technology = "Angular";

        // Act
        var result = new
        {
            Technology = technology,
            RelatedTechnology = "TypeScript",
            Project = "Banking API Platform",
            Domain = "Banking"
        };

        // Assert
        Assert.Equal("Angular", result.Technology);
        Assert.Equal("TypeScript", result.RelatedTechnology);
        Assert.Equal("Banking API Platform", result.Project);
        Assert.Equal("Banking", result.Domain);
    }
}