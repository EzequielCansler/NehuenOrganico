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
        private readonly  IItemRepository _itemRepo;

        private readonly OrderService _orderService;


        public OrdersController(IOrderRepository orderRepo, IProductRepository produRepo, OrderService orderService, IItemRepository itemRepo)
        {
            _orderRepo = orderRepo;           
            _produRepo = produRepo;
            _orderService = orderService;
            _itemRepo = itemRepo;
        }

        public IActionResult AddItem(int productId, int qty = 1,int redirect=0)
        {

            var cartCount = _orderRepo.AddItem(productId,qty);
            if (redirect == 0)
                return Ok(cartCount);        
            return RedirectToAction("GetUserCart");
        }

        public IActionResult RemoveItem(int id)
        {
                var cartCount = _orderRepo.RemoveItem(id);
            return RedirectToAction("Index","Orders");
        }


        public IActionResult GetUserCart()
        {
            var cart = _orderRepo.GetUserCart();
            return Json(cart);
        }

        //public IActionResult GetTotalItemInCart(int orderId)
        //{        
        //    int cartItem = _orderRepo.GetCartItemCount(orderId);
        //    return Json(cartItem);
        //}
        


        [HttpGet]
        public IActionResult Index()
        {
            List<Order> order = new();
            order = _orderRepo.GetUserOrders();


            ViewData["Items"] = _itemRepo.GetAllItems();
            return View(order);
        }
       
        public IActionResult Add()
        {
            Order order = new();
           
            var items = _orderRepo.GetUserCart().ToList();
            var products = new List<Product>();
            foreach (OrderItem item in items)
            {
                Product product = _produRepo.GetById(item.ProductId);
                products.Add(product);
            }
            int? orderId = items.First().OrderId;
            int? stateId = _orderRepo.GetStateIdByOrderId(orderId);
            if(stateId == 1)
            {
            ViewData["OrderItem"] = items;          
            ViewData["Products"] = products;
            ViewData["OrderId"] = items.FirstOrDefault()?.OrderId;

            return View(order);

            }
            else
            {
                return RedirectToAction("Index", "Orders");
            }
            
        }
        [HttpPost]
        public IActionResult NewOrder(int id, string shipping, string comments)
        {
            bool result = _orderService.CreateOrder(id, shipping, comments);
            if (!result)
                throw new Exception("Error en el servidor");
            return RedirectToAction("Index", "Orders");
        }

    }
}
