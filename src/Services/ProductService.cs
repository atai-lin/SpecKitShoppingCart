using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models;
using ShoppingCart.Models.Dtos;

namespace ShoppingCart.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly Microsoft.Extensions.Logging.ILogger<ProductService> _logger;

        public ProductService(AppDbContext context, Microsoft.Extensions.Logging.ILogger<ProductService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PaginatedList<ProductDto>> GetProductsAsync(
            string? q, 
            int[]? categoryIds, 
            string? sortBy, 
            bool isDesc, 
            int pageIndex, 
            int pageSize)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            _logger.LogInformation("Searching products with query: {Query}, SortBy: {SortBy}, PageIndex: {PageIndex}", q, sortBy, pageIndex);

            var query = _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(q))
            {
                // Priority: Exact match on Name
                // In a real app with 10k items, we'd use a more sophisticated approach.
                // For now, we use a simple LINQ query.
                query = query.Where(p => p.Name == q || p.Name.Contains(q) || (p.Description != null && p.Description.Contains(q)));
            }

            if (categoryIds != null && categoryIds.Length > 0)
            {
                query = query.Where(p => categoryIds.Contains(p.CategoryId));
            }

            // Sorting
            query = sortBy switch
            {
                "Price" => isDesc ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                "SalesVolume" => isDesc ? query.OrderByDescending(p => p.SalesVolume) : query.OrderBy(p => p.SalesVolume),
                "CreatedTime" => isDesc ? query.OrderByDescending(p => p.CreatedTime) : query.OrderBy(p => p.CreatedTime),
                _ => query.OrderByDescending(p => p.CreatedTime) // Default
            };

            var productDtos = query.Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.SalesVolume,
                p.ImageUrl,
                p.CreatedTime,
                p.Category.Name
            ));

            var result = await PaginatedList<ProductDto>.CreateAsync(productDtos, pageIndex, pageSize);

            stopwatch.Stop();
            _logger.LogInformation("Product search completed in {Elapsed}ms. Found {Count} items.", stopwatch.ElapsedMilliseconds, result.Count);

            return result;
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            return await _context.Categories
                .AsNoTracking()
                .Select(c => new CategoryDto(c.Id, c.Name, c.Description))
                .ToListAsync();
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var p = await _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (p == null) return null;

            return new ProductDto(
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.SalesVolume,
                p.ImageUrl,
                p.CreatedTime,
                p.Category.Name
            );
        }
    }
}
