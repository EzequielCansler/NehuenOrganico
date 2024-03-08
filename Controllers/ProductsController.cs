using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NehuenOrganico.Models;
using NehuenOrganico.Repositories;

namespace NehuenOrganico.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            var products = ProductsRepository.GetProducts();
            return View(products);
        }
        [Authorize]
        
        public IActionResult Edit(int? id)
        {
            var product = new Product { ProductId = id.HasValue?id.Value:0 };

            return View(product);
        }
    }
}
