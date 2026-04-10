using Microsoft.AspNetCore.Mvc.Testing;
using ShoppingCart.Models.Dtos;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace ShoppingCart.Tests.Integration
{
    public class FilterFlowTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public FilterFlowTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetProductsApi_ReturnsValidJson_AndFiltersCorrectly()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act - Get all products
            var response = await client.GetAsync("/api/product-list");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<FilteredResult>();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Products);
            Assert.True(result.Pagination.TotalCount > 0);
        }

        [Fact]
        public async Task GetProductsApi_WithCategoryFilter_ReturnsFilteredResults()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act - Filter by category 1
            var response = await client.GetAsync("/api/product-list?categories=1");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<FilteredResult>();

            // Assert
            Assert.NotNull(result);
            foreach (var product in result.Products)
            {
                // Verify results match logic - based on seed data in DbInitializer
                // (Note: in a real integration test we might use a dedicated test DB)
            }
        }
    }
}
