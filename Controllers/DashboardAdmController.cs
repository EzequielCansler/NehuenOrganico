using Microsoft.AspNetCore.Mvc;
using NehuenOrganico.Repositories.Interfaces;
using NehuenOrganico.Services;

namespace NehuenOrganico.Controllers
{
    public class DashboardAdmController : Controller
    {
        private readonly IDashboardAdmRepository _admRepo;
       

        public DashboardAdmController(IDashboardAdmRepository admRepo)
        {
            _admRepo = admRepo;
        }

        public IActionResult Index()
        {
            var order = _admRepo.GetAllOrderWithStatePending();
            return View(order);
        }
        public IActionResult AdmProduct()
        {
            var item = _admRepo.GetAllProductsWithStatePending();
            return View(item);
        }

        public IActionResult AdmOrder()
        {
            var order = _admRepo.GetAllOrderWithStatePending();
            return View(order);
        }

    }
}
