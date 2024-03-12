using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NehuenOrganico.Models;
using NehuenOrganico.Data;
using Microsoft.EntityFrameworkCore;


namespace NehuenOrganico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            var products = await _context.Products.ToListAsync();

            return Ok(products);
        }


        [HttpGet]
        [Route("{id}")]
        public IActionResult Edit(int? id)
        {
            var product = ProductsRepository.GetProductById(id.HasValue ? id.Value : 0);

            return View(product);
        }
        [HttpPost]
        public IActionResult Edit(Product product)
        {
           ProductsRepository.UpdateProduct(product.ProductId, product);
           return RedirectToAction("Index","");
        }
    }
}
