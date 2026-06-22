using mvcApp.Dto;

namespace mvcApp.Tests;

public class ProductDtoTests
{
    [Fact]
    public void ProductDto_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var product = new ProductDto
        {
            ID = 1,
            ProductName = "Laptop",
            Description = "A powerful laptop",
            Price = 999.99m,
            Color = "Silver"
        };

        // Assert
        Assert.Equal(1, product.ID);
        Assert.Equal("Laptop", product.ProductName);
        Assert.Equal(999.99m, product.Price);
        Assert.Equal("Silver", product.Color);
    }

    [Fact]
    public void ProductDto_PriceShouldBePositive()
    {
        // Arrange
        var product = new ProductDto
        {
            Price = 499.99m
        };

        // Assert
        Assert.True(product.Price > 0);
    }
}