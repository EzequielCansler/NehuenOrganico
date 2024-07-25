using Microsoft.AspNetCore.Mvc;
using NehuenOrganico.Models;
using NehuenOrganico.Repositories.Interfaces;
using NehuenOrganico.Services;

namespace NehuenOrganico.Controllers
{
    public class ItemController : Controller
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IProductRepository _produRepo;
        private readonly IItemRepository _itemRepo;



        public ItemController(IOrderRepository orderRepo, IProductRepository produRepo, IItemRepository itemRepo)
        {
            _orderRepo = orderRepo;
            _produRepo = produRepo;
            _itemRepo = itemRepo;
        }
        public IActionResult ItemCount()
        {
            try
            {
                int cartItem = _itemRepo.GetItemCount();
                return Json(cartItem);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
