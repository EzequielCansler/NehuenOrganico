using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NehuenOrganico.Data;
using NehuenOrganico.Models;
using NehuenOrganico.Repositories.Interfaces;

namespace NehuenOrganico.Repositories
{
    public class DashboardAdmRepository : IDashboardAdmRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IItemRepository _itemRepo;


        public DashboardAdmRepository(AppDbContext appDbContext, UserManager<AppUser> userManager, IHttpContextAccessor contextAccessor, IItemRepository itemRepo)
        {
            _appDbContext = appDbContext;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _itemRepo = itemRepo;
        }
        public List<Order> GetAllOrderWithStatePending()
        {
            List<Order> order = _appDbContext.Order                
                .Include(x => x.State)
                .Include(x => x.AppUser)
                .Include(x=> x.Items)
                .ThenInclude(x => x.Product)
                .Where(x => x.StateId == 2) // Pedido
                .ToList();

            return order;
        }
        public List<dynamic> GetAllProductsWithStatePending()
        {
            var orders = _appDbContext.Order
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .Where(x => x.StateId == 2) // Pedido
                .ToList(); // Obtiene los datos

            // Agrupación en memoria
            var groupedItems = orders
                .SelectMany(order => order.Items.Select(item => new
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Shipping = order.Shipping,
                    Quantity = item.Quantity
                }))
                .GroupBy(x => new { x.ProductId, x.ProductName, x.Shipping }) // Agrupación
                .Select(g => new
                {
                    ProductName = g.Key.ProductName,
                    Shipping = g.Key.Shipping,
                    TotalQuantity = g.Sum(x => x.Quantity)
                })
                .ToList<dynamic>();

            return groupedItems;
        }


    }
}
