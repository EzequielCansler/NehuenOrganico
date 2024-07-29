using Microsoft.AspNetCore.Identity;
using NehuenOrganico.Data;
using NehuenOrganico.Models;
using NehuenOrganico.Repositories.Interfaces;

namespace NehuenOrganico.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<AppUser> _userManager;

        public ItemRepository(AppDbContext appDbContext, UserManager<AppUser> userManager, IHttpContextAccessor contextAccessor)
        {
            _appDbContext = appDbContext;
            _contextAccessor = contextAccessor;
            _userManager = userManager;

        }

        public List<OrderItem> GetAllItemsFormID (int orderId)
        {
            List<OrderItem> item = _appDbContext.OrderItem.Where( x => x.OrderId == orderId).ToList();
            return item;
        }
        public List<OrderItem> GetAllItems()
        {
            List<OrderItem> item = _appDbContext.OrderItem.ToList();
            return item;
        }
        public int GetItemCount()
        {
            try
            {
                string userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("El ID de usuario no es válido.");
                }
                var order = _appDbContext.Order.FirstOrDefault(x=> x.StateId == 1 && x.Id == userId);
                
                var totalQuantity = _appDbContext.OrderItem
                    .Where(x => x.OrderId == order.OrderId)
                    .Sum(x => x.Quantity);

                return totalQuantity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving item count: {ex.Message}");
            }
        }



        public string GetUserId()
        {
            var user = _contextAccessor.HttpContext?.User;
            var userId = _userManager.GetUserId(user);
            return userId;
        }
    }
}
