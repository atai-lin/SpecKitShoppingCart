using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ShoppingCart.Data;
using ShoppingCart.Models;
using ShoppingCart.Models.Dtos;
using ShoppingCart.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ShoppingCart.Tests.Unit
{
    public class ProductServiceTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
            var context = new AppDbContext(options);
            
            // Seed data
            var category = new Category { Id = 1, Name = "Electronics" };
            context.Categories.Add(category);
            context.Products.Add(new Product { Id = 1, Name = "Phone", Price = 500, CategoryId = 1, Material = "Metal", Color = "Black" });
            context.Products.Add(new Product { Id = 2, Name = "Laptop", Price = 1000, CategoryId = 1, Material = "Plastic", Color = "Silver" });
            context.SaveChanges();
            
            return context;
        }

        [Fact]
        public async Task GetFilteredProductsAsync_ReturnsAllProducts_WhenNoFilters()
        {
            // Arrange
            using var context = GetDbContext();
            var logger = new Mock<ILogger<ProductService>>();
            var service = new ProductService(context, logger.Object);
            var request = new FilterRequest();

            // Act
            var result = await service.GetFilteredProductsAsync(request);

            // Assert
            Assert.Equal(2, result.Products.Count);
        }

        [Fact]
        public async Task GetFilteredProductsAsync_AppliesCategoryFilter()
        {
            // Arrange
            using var context = GetDbContext();
            var logger = new Mock<ILogger<ProductService>>();
            var service = new ProductService(context, logger.Object);
            var request = new FilterRequest { Categories = new List<int> { 1 } };

            // Act
            var result = await service.GetFilteredProductsAsync(request);

            // Assert
            Assert.All(result.Products, p => Assert.Equal("Electronics", p.CategoryName));
        }

        [Fact]
        public async Task GetFilteredProductsAsync_AppliesOrLogicWithinGroup()
        {
            // Arrange
            using var context = GetDbContext();
            var logger = new Mock<ILogger<ProductService>>();
            var service = new ProductService(context, logger.Object);
            var request = new FilterRequest { Materials = new List<string> { "Metal", "Plastic" } };

            // Act
            var result = await service.GetFilteredProductsAsync(request);

            // Assert
            Assert.Equal(2, result.Products.Count);
        }
    }
}
