using SogetiTODO.Validations;

namespace SogetiTODO.Tests.Unit.ControllersTests;

public class DateValidationTests
{
    private readonly DateValidation _validation = new();

    [Fact]
    public void IsValid_ReturnsTrue_WhenDateIsWithinRange()
    {
        // Arrange
        var date = DateTime.Now.AddDays(1);

        // Act
        var result = _validation.IsValid(date);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenDateIsBeforeToday()
    {
        // Arrange
        var date = DateTime.Now.AddDays(-1);

        // Act
        var result = _validation.IsValid(date);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenDateIsMoreThanTwoYearsInTheFuture()
    {
        // Arrange
        var date = DateTime.Now.AddYears(3);

        // Act
        var result = _validation.IsValid(date);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenValueIsNotADate()
    {
        // Arrange
        var value = "not a date";

        // Act
        var result = _validation.IsValid(value);

        // Assert
        Assert.False(result);
    }
}