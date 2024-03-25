using Microsoft.AspNetCore.Mvc;
using NehuenOrganico.Data;
using NehuenOrganico.Models;
using NehuenOrganico.Repositories.Interfaces;


namespace NehuenOrganico.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IOrderRepository _orderRepo;

        //public OrderController(AppDbContext appDbContext, IOrderRepository orderRepo)
        //{
        //    _appDbContext = appDbContext;
        //    _orderRepo = orderRepo;
        //}

        public IActionResult Create()
        {
            var result = _appDbContext;
            return View();
        }
       
    }
}
