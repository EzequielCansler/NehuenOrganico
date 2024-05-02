using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NehuenOrganico.Data;
using NehuenOrganico.Models;
using NehuenOrganico.Repositories.Interfaces;
namespace NehuenOrganico.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IProductRepository _produRepo;
       
       
        public OrdersController(IOrderRepository orderRepo, IProductRepository produRepo)
        {
            _orderRepo = orderRepo;           
            _produRepo = produRepo;
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
            return View(cart);
        }

        public IActionResult GetTotalItemInCart(string userId)
        {

            int cartItem = _orderRepo.GetCartItemCount(userId);
            return Json(cartItem);
        }
        public IActionResult CheckOut()
        {
            bool isCheckedOut = _orderRepo.DoCheckOut("martes","ningun comentario");
            if (!isCheckedOut)
                throw new Exception("Error en el servidor");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Index()
        {
            var orderItem = _orderRepo.GetUserCart();
            var products = new List<Product>();
            foreach(OrderItem item in orderItem)
            {
                Product product = _produRepo.GetById(item.ProductId);
                products.Add(product);
            }

            ViewData["Products"] = products;

            return View(orderItem);
        }

    }
}
