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
            string? sortBy, 
            bool isDesc, 
            int pageIndex, 
            int pageSize);

        Task<IEnumerable<CategoryDto>> GetCategoriesAsync();

        Task<ProductDto?> GetProductByIdAsync(int id);
    }
}
