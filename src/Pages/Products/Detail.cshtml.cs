using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingCart.Models.Dtos;
using ShoppingCart.Services;

namespace ShoppingCart.Pages.Products
{
    public class DetailModel : PageModel
    {
        private readonly IProductService _productService;

        public DetailModel(IProductService productService)
        {
            _productService = productService;
        }

        public ProductDto? Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _productService.GetProductByIdAsync(id);

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
