using Microsoft.AspNetCore.Identity;
using NehuenOrganico.Data;
using NehuenOrganico.Models;
using NehuenOrganico.Repositories.Interfaces;




namespace NehuenOrganico.Repositories
{
    public class OrderRepository : IOrderRepository // Herencia
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IItemRepository _itemRepo;


        public OrderRepository(AppDbContext appDbContext, UserManager<AppUser> userManager, IHttpContextAccessor contextAccessor, IItemRepository itemRepo)
        {
            _appDbContext = appDbContext;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _itemRepo = itemRepo;
        }


        public int AddItem(int ProductId, int qty)
        {
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("Usuario no conectado");

                Order order = GetOrder(userId);
                int? state = GetStateIdByOrderId(order.OrderId);
                if (order == null || state == 1)
                {
                    if (order == null)
                    {
                        order = new Order // crea el pedido si no existe
                        {
                            Id = userId,

                            StateId = 1 // Pendiente
                        };
                        _appDbContext.Add(order);
                        _appDbContext.SaveChanges();
                    }
                    
                    // agrega los items del pedido
                    var orderItem = _appDbContext.OrderItem.FirstOrDefault(x => x.OrderId == order.OrderId && x.ProductId == ProductId);
                    if (orderItem != null)
                    {
                        orderItem.Quantity += qty;
                    }
                    else
                    {
                        Product product = _appDbContext.Product.FirstOrDefault(x => x.ProductId == ProductId);
                        orderItem = new OrderItem
                        {
                            ProductId = ProductId,
                            OrderId = order.OrderId,
                            Quantity = qty,
                            UnitPrice = product.Price
                        };
                        _appDbContext.OrderItem.Add(orderItem);
                    }
                    _appDbContext.SaveChanges();
                    var cartItemCount = GetCartItemCount(order.OrderId);
                    return cartItemCount;
                }
                else {
                    return 0;
                }
            }
            catch
            {
                throw new Exception("error al agregar el item");
            }

        } // done
        public int RemoveItem(int id)
        {
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("Usuario no conectado");
                Order order = GetOrder(userId);
                if (order == null)
                    throw new Exception("Carrito invalido");
                // order item
                var cartItem = _appDbContext.OrderItem.FirstOrDefault(x => x.OrderId == order.OrderId && x.ProductId == id);
                if (cartItem == null)
                    throw new Exception("No tiene items el carrito");
                else if (cartItem.Quantity == 1)
                    _appDbContext.OrderItem.Remove(cartItem);
                else
                    cartItem.Quantity = cartItem.Quantity - 1;
                _appDbContext.SaveChanges();
                var cartItemCount = GetCartItemCount(order.OrderId);
                return cartItemCount;
            }
            catch (Exception ex)
            {
                return -1;
            }
        } // done
        public Order GetOrder(string userId)
        {
            Order order = _appDbContext.Order.FirstOrDefault(x => x.Id == userId);
            return order;
        }// done
        public string GetUserId()
        {
            var user = _contextAccessor.HttpContext.User;
            var userId = _userManager.GetUserId(user);
            return userId;
        } // done
        public int GetCartItemCount(int orderId)
        {

            var totalQuantity = _appDbContext.OrderItem
            .Where(x => x.OrderId == orderId)
            .Sum(x => x.Quantity);

            return totalQuantity;
        }// done
        public List<OrderItem> GetUserCart()
        {
            var userId = GetUserId();
            if (userId == null)
            {
                throw new Exception("No se encontro el usuario");
            }
            Order order = _appDbContext.Order.FirstOrDefault(x => x.Id == userId);

            
                return _appDbContext.OrderItem
                    .Where(item => item.OrderId == order.OrderId)
                    .ToList();

            
            
        }// done
        public bool CreateOrder(int id, string shippingDetails, string comments, double total, DateTime date)
        {
            using var transaction = _appDbContext.Database.BeginTransaction();
            {
                try
                {
                    string userId = GetUserId();
                    var user = _appDbContext.Users.FirstOrDefault(x => x.Id == userId);
                    if (string.IsNullOrEmpty(userId))
                        throw new Exception("Usuario no conectado");
                    Order order = GetOrder(userId);
                    if (order is null)
                        throw new Exception("No tiene Carrito");
                    List<OrderItem> items = _appDbContext.OrderItem.Where(x => x.OrderId == order.OrderId).ToList(); ; // cambiar nombre

                    if (items.Count == 0)
                        throw new Exception("carrito vacio");

                    order.CreateDate = date;
                    order.StateId = 2; // Comprado
                    order.Name = user.Name;
                    order.Address = user.Address;
                    order.PhoneNumber = user.PhoneNumber;
                    order.Shipping = shippingDetails;
                    order.Comments = comments;
                    order.Total = total;

                    _appDbContext.SaveChanges();

                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public List<Order> GetUserOrders()
        {
            List<Order> order = new();
            var user = GetUserId();
            order = _appDbContext.Order.Where(x => x.Id == user).ToList();

            return order;
        }
        public int? GetStateIdByOrderId(int? orderId)
        {
            int stateId = _appDbContext.Order
                .Where(x => x.OrderId == orderId)
                .Select(x => (int)x.StateId)
                .FirstOrDefault();

            return stateId;
        }
    }
}