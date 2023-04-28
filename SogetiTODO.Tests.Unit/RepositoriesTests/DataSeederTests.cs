using SogetiTODO.Repositories.Implementations;

namespace SogetiTODO.Tests.Unit.RepositoriesTests;

public class DataSeederTests
{
    // Might not be needed...
    [Fact]
    public async Task SeedTodos_ReturnsExpectedTodosList()
    {
        // Arrange
        var dataSeeder = new DataSeeder();

        // Act
        var todos = await dataSeeder.SeedTodos();

        // Assert
        Assert.NotNull(todos);
        Assert.Equal(4, todos.Count);
    }
}