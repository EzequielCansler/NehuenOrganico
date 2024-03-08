using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NehuenOrganico.Repositories;

namespace NehuenOrganico.Controllers
{

    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            var products = ProductsRepository.GetProducts();
            
            return View(products);
        }
    }
}
