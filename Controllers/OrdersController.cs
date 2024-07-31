using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NehuenOrganico.Data;
using NehuenOrganico.Models;
using NehuenOrganico.Repositories.Interfaces;
using NehuenOrganico.Services;

namespace NehuenOrganico.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IProductRepository _produRepo;
        private readonly IItemRepository _itemRepo;
        private readonly ICategoryRepository _cateRepo;

        private readonly OrderService _orderService;


        public OrdersController(IOrderRepository orderRepo, IProductRepository produRepo, OrderService orderService, IItemRepository itemRepo, ICategoryRepository cateRepo)
        {
            _orderRepo = orderRepo;
            _produRepo = produRepo;
            _orderService = orderService;
            _itemRepo = itemRepo;
            _cateRepo = cateRepo;
        }

        public IActionResult AddItem(int productId, int qty = 1, int redirect = 0)
        {

            var cartCount = _orderRepo.AddItem(productId, qty);
            if (redirect == 0)
                return Ok(cartCount);
            return RedirectToAction("GetUserCart");
        }
        public IActionResult Add()
        {
           
            Order order = _orderRepo.GetPendingOrder();
          
            return View(order);
        }
        public IActionResult RemoveItem(int id)
        {
            var cartCount = _orderRepo.RemoveItem(id);
            return RedirectToAction("Add", "Orders");
        }

        public IActionResult GetUserCart()
        {
            var cart = _orderRepo.GetUserCart();
            return Json(cart);
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Order> order = new();
            order = _orderRepo.GetUserOrders();

            return View(order);
        }

        [HttpPost]
        public IActionResult NewOrder(int id, string shipping, string comments)
        {
            bool result = _orderService.CreateOrder(id, shipping, comments);
            if (!result)
                throw new Exception("Error en el servidor");
            return RedirectToAction("Index", "Orders");
        }

        [HttpPost]
        public IActionResult CancelOrder(int OrderId)
        {
            bool isStateChanged = _orderRepo.ChangeState(OrderId, 5);
            if (!isStateChanged)
                return View("Error");

            var refererUrl = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(refererUrl))
            {
                return Redirect(refererUrl);
            }
            return RedirectToAction("Index", "Orders");

        }
        [HttpPost]
        public IActionResult PaidOrder(int OrderId)
        {
            bool isStateChanged = _orderRepo.ChangeState(OrderId, 3);
            if (!isStateChanged)
                return View("Error");

            var refererUrl = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(refererUrl))
            {
                return Redirect(refererUrl);
            }
            return RedirectToAction("Index", "Orders");

        }
        [HttpPost]
        public IActionResult deliveredOrder(int OrderId)
        {
            bool isStateChanged = _orderRepo.ChangeState(OrderId, 4);
            if (!isStateChanged)
                return View("Error");

            var refererUrl = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(refererUrl))
            {
                return Redirect(refererUrl);
            }
            return RedirectToAction("Index", "Orders");

        }
    }
}
