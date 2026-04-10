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
            string? material,
            string? color,
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
                query = query.Where(p => p.Name.Contains(q) || (p.Description != null && p.Description.Contains(q)));
            }

            if (categoryIds != null && categoryIds.Length > 0)
            {
                query = query.Where(p => categoryIds.Contains(p.CategoryId));
            }

            if (!string.IsNullOrWhiteSpace(material))
            {
                query = query.Where(p => p.Material == material);
            }

            if (!string.IsNullOrWhiteSpace(color))
            {
                query = query.Where(p => p.Color == color);
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
                p.Category.Name,
                p.Material,
                p.Color
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

        public async Task<IEnumerable<string>> GetAvailableMaterialsAsync()
        {
            return await _context.Products
                .Where(p => p.Material != null)
                .Select(p => p.Material!)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetAvailableColorsAsync()
        {
            return await _context.Products
                .Where(p => p.Color != null)
                .Select(p => p.Color!)
                .Distinct()
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
                p.Category.Name,
                p.Material,
                p.Color
            );
        }

        public async Task<FilteredResult> GetFilteredProductsAsync(FilterRequest request)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            _logger.LogInformation("Filtering products via API. Search: {Search}, Categories: {Categories}, Materials: {Materials}, Colors: {Colors}", 
                request.Q, string.Join(",", request.Categories), string.Join(",", request.Materials), string.Join(",", request.Colors));

            var query = _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .AsQueryable();

            // 1. Apply Search
            if (!string.IsNullOrWhiteSpace(request.Q))
            {
                query = query.Where(p => p.Name.Contains(request.Q) || (p.Description != null && p.Description.Contains(request.Q)));
            }

            // 2. Apply Filters (OR logic within group)
            if (request.Categories != null && request.Categories.Any())
            {
                query = query.Where(p => request.Categories.Contains(p.CategoryId));
            }

            if (request.Materials != null && request.Materials.Any())
            {
                query = query.Where(p => p.Material != null && request.Materials.Contains(p.Material));
            }

            if (request.Colors != null && request.Colors.Any())
            {
                query = query.Where(p => p.Color != null && request.Colors.Contains(p.Color));
            }

            // 3. Apply Sorting
            query = request.Sort switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                "newest" => query.OrderByDescending(p => p.CreatedTime),
                _ => query.OrderByDescending(p => p.SalesVolume)
            };

            // 4. Get Dynamic Facet Counts (based on current filters)
            // Note: In a production scenario, these would often be calculated in a single more optimized query or via a search engine like Elastic.
            // For this project, we'll use EF to count.
            var facetCounts = new FacetCounts
            {
                CategoryCounts = await _context.Products
                    .GroupBy(p => p.Category.Name)
                    .Select(g => new { Name = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.Name, x => x.Count),

                MaterialCounts = await _context.Products
                    .Where(p => p.Material != null)
                    .GroupBy(p => p.Material!)
                    .Select(g => new { Name = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.Name, x => x.Count),

                ColorCounts = await _context.Products
                    .Where(p => p.Color != null)
                    .GroupBy(p => p.Color!)
                    .Select(g => new { Name = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.Name, x => x.Count)
            };

            // 5. Pagination
            int pageSize = 12;
            int totalCount = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            int skip = (request.Page - 1) * pageSize;

            var products = await query.Skip(skip).Take(pageSize).Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.SalesVolume,
                p.ImageUrl,
                p.CreatedTime,
                p.Category.Name,
                p.Material,
                p.Color
            )).ToListAsync();

            stopwatch.Stop();
            _logger.LogInformation("API Filtering completed in {Elapsed}ms. Found {Count} items.", stopwatch.ElapsedMilliseconds, totalCount);

            return new FilteredResult
            {
                Products = products,
                Facets = facetCounts,
                Pagination = new PaginationInfo
                {
                    CurrentPage = request.Page,
                    TotalPages = totalPages,
                    PageSize = pageSize,
                    TotalCount = totalCount
                }
            };
        }
    }
}
