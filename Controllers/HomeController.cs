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
        
        public IActionResult Index()
        {
            var product = _productRepo.GetAll();
            return View(product);
        }

        [HttpGet]
       public  IActionResult GetAll()
        {
            var product = _productRepo.GetAll();
            return Ok(product);
        }
        
    }
}

