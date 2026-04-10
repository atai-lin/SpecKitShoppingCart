using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingCart.Models;
using ShoppingCart.Models.Dtos;

namespace ShoppingCart.Services
{
    public interface IProductService
    {
        Task<PaginatedList<ProductDto>> GetProductsAsync(
            string? q, 
            int[]? categoryIds, 
            string? material,
            string? color,
            string? sortBy, 
            bool isDesc, 
            int pageIndex, 
            int pageSize);

        Task<IEnumerable<CategoryDto>> GetCategoriesAsync();

        Task<IEnumerable<string>> GetAvailableMaterialsAsync();

        Task<IEnumerable<string>> GetAvailableColorsAsync();

        Task<ProductDto?> GetProductByIdAsync(int id);

        Task<FilteredResult> GetFilteredProductsAsync(FilterRequest request);
    }
}
