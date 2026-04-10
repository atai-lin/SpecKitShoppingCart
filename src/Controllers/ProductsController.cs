using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models.Dtos;
using ShoppingCart.Services;
using System.Threading.Tasks;

namespace ShoppingCart.Controllers
{
    [ApiController]
    [Route("api/product-list")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<FilteredResult>> GetProducts([FromQuery] FilterRequest request)
        {
            var result = await _productService.GetFilteredProductsAsync(request);
            return Ok(result);
        }
    }
}
