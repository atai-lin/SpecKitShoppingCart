using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingCart.Models;
using ShoppingCart.Models.Dtos;
using ShoppingCart.Services;

namespace ShoppingCart.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;

        public IndexModel(IProductService productService)
        {
            _productService = productService;
        }

        public PaginatedList<ProductDto> Products { get; set; } = default!;
        public IEnumerable<CategoryDto> Categories { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? Q { get; set; }

        [BindProperty(SupportsGet = true)]
        public int[]? CategoryIds { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SortBy { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool IsDesc { get; set; } = true;

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        public async Task OnGetAsync()
        {
            const int pageSize = 20;

            Products = await _productService.GetProductsAsync(
                Q, 
                CategoryIds, 
                SortBy, 
                IsDesc, 
                PageIndex, 
                pageSize);

            Categories = await _productService.GetCategoriesAsync();
        }
    }
}
