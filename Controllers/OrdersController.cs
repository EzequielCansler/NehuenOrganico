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

        public OrdersController(IOrderRepository orderRepo)
        {
            _orderRepo= orderRepo;
        }

        public IActionResult AddItem(int productId, int qty = 1,int redirect=0)
        {
            var cartCount = _orderRepo.AddItem(productId,qty);
            if (redirect == 0)
                return Ok(cartCount);        
            return RedirectToAction("GetUserCart");
        }

        public IActionResult RemoveItem(int productId)
        {
            var cartCount = _orderRepo.RemoveItem(productId);
            return RedirectToAction("GetUserCart");
        }

        public IActionResult GetUserCart()
        {
            var cart = _orderRepo.GetUserCart();
            return View(cart);
        }

        public IActionResult GetTotalItemInCart(string userId)
        {

            int cartItem = _orderRepo.GetCartItemCount(userId);
            return View(cartItem);
        }

    }
}
