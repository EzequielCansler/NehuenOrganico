using Microsoft.AspNetCore.Mvc;
using NehuenOrganico.Data;
using NehuenOrganico.Models;
using NehuenOrganico.Repositories.Interfaces;
namespace NehuenOrganico.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IProductRepository _producRepo;

        public OrdersController(AppDbContext context, IProductRepository producRepo)
        {
            _context = context;
            _producRepo = producRepo;

        }

       
       
    }
}
