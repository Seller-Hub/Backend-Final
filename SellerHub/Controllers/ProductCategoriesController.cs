using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SellerHub.DTOs;
using SellerHub.Services;

namespace SellerHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly ProductService _productService;
        public ProductCategoriesController(ProductService productService)
        {
            _productService = productService;
        }

        // POST: /api/productcategories
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateProductCategoryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest(new { message = "Category name is required" });

            var category = await _productService.CreateCategoryAsync(dto.Name);
            if (category == null)
                return BadRequest(new { message = "Category already exists" });

            return Ok(category);
        }

        // GET: /api/productcategories
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _productService.GetAllCategoriesAsync();
            return Ok(categories);
        }
    }
}
