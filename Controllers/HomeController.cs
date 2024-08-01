using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NehuenOrganico.Data;
using NehuenOrganico.Models;
using NehuenOrganico.Repositories;
using NehuenOrganico.Repositories.Interfaces;


namespace NehuenOrganico.Controllers
{

    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IProductRepository _productRepo;
        


        public HomeController(AppDbContext context, IProductRepository productRepository)
        {
            _productRepo = productRepository;
            _context = context;
            
        }
        
        public IActionResult Index(string searchString, string category)
        {
            var product = _productRepo.GetAll();
            
            if (!String.IsNullOrEmpty(searchString))
            {
                product = product.Where(x => x.Name.Contains(searchString)).ToList();
            }
            if (!String.IsNullOrEmpty(category))
            {
                product = product.Where(x => x.Category.CategoryName == category).ToList();
            }

            product = product.OrderByDescending(x => x.ProductId).ToList();

            return View(product);
        }

        
    }
}

