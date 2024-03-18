using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NehuenOrganico.Models;
using NehuenOrganico.Data;
using NehuenOrganico.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace NehuenOrganico.Controllers
{

    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _CategoryRepo;


        public ProductsController(AppDbContext context, IProductRepository productRepo, ICategoryRepository categoryRepo)
        {
            _productRepo = productRepo;
            _context = context;
            _CategoryRepo = categoryRepo;
        }

        [HttpGet]
        
        [Authorize]
        public IActionResult Edit(int? id)
        {
            var product = _productRepo.GetById(id.HasValue ? id.Value : 0);
            var categories = _CategoryRepo.GetAllCategories();

            ViewData["Categories"] = categories;

            return View(product);
        }

        [HttpPost]
        
        public IActionResult Edit(Product product)
        {
           
            _productRepo.Update(product);
            return RedirectToAction("Index","Home");
                   
        }
        [HttpGet]
        public IActionResult Add()
        {
            var categories = _CategoryRepo.GetAllCategories();

            ViewData["Categories"] = categories;
            return View();
        }
        [HttpPost]
        public IActionResult Add(Product product)
        {
            
                var newProduct = _productRepo.AddProduct(product);

                if (newProduct != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return BadRequest("Error al agregar el producto.");
                }
            
           
        }
        public IActionResult Delete([FromRoute] int id)
        {
            _productRepo.DeleteProduct(id);
            

            return RedirectToAction("Index", "Home");
        }

    }
}
