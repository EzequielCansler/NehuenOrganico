using Microsoft.AspNetCore.Identity;
using NehuenOrganico.Data;
using NehuenOrganico.Models;
using NehuenOrganico.Repositories.Interfaces;
using System.Linq;



namespace NehuenOrganico.Repositories
{
    public class OrderRepository : IOrderRepository
    {       
        private readonly AppDbContext _appDbContext;     
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public OrderRepository(AppDbContext appDbContext,UserManager<AppUser> userManager, IHttpContextAccessor contextAccessor)
        {
            _appDbContext = appDbContext;
            _contextAccessor = contextAccessor;         
            _userManager = userManager;
        }


        public int AddItem(int ProductId,int qty)
        {
            string userId = GetUserId();           
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("Usuario no conectado");
                Order order = GetItem(userId);
                if(order == null)
                {
                    order = new Order
                    {
                        Id = userId,
                    };
                    _appDbContext.Add(order);
                    _appDbContext.SaveChanges();
                }
                // order item
                var orderItem = _appDbContext.OrderItem.FirstOrDefault(x => x.OrderId == order.OrderId && x.ProductId == ProductId);
                if(orderItem != null)
                {
                    orderItem.Quantity += qty;
                }
                else
                {
                    Product produ = _appDbContext.Product.FirstOrDefault(x => x.ProductId == ProductId);
                    orderItem = new OrderItem
                    {
                        ProductId = ProductId,
                        OrderId = order.OrderId,
                        Quantity = qty,
                        UnitPrice = produ.Price
                    };
                    _appDbContext.OrderItem.Add(orderItem);
                }
                _appDbContext.SaveChanges();
                               
            }
            catch
            {
                throw new Exception("error al agregar el item");
            }
            var cartItemCount = GetCartItemCount(userId);
            return cartItemCount;
        } // done
        public int RemoveItem(int id)
        {            
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("Usuario no conectado");
                Order order = GetItem(userId);
                if (order == null)
                    throw new Exception("Carrito invalido");
                // order item
                var cartItem = _appDbContext.OrderItem.FirstOrDefault(x => x.OrderId == order.OrderId && x.ProductId == id);
                if (cartItem == null)
                    throw new Exception("No tiene items el carrito");
                else if(cartItem.Quantity == 1) 
                    _appDbContext.OrderItem.Remove(cartItem);
                else 
                    cartItem.Quantity = cartItem.Quantity - 1;
                _appDbContext.SaveChanges();                
            }
            catch (Exception ex)
            {
            }
            var cartItemCount = GetCartItemCount(userId);
            return cartItemCount;
        } // done
        public Order GetItem(string userId) 
        {
            Order item = _appDbContext.Order.FirstOrDefault(x => x.Id == userId);
            return item;
        }// done
        public string GetUserId()
        {
            var user = _contextAccessor.HttpContext.User;
            var userId = _userManager.GetUserId(user);
            return userId;
        } // done
        public int GetCartItemCount(string userId) 
        {
            if (!string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            var data =  (from Order in _appDbContext.Order
                       join OrderItem in _appDbContext.OrderItem
                       on Order.OrderId equals OrderItem.OrderId
                       select OrderItem.Quantity 
                       ).Sum();
                               
            return data;
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
        public bool DoCheckOut (string shippingDetails, string comments )
        {
            using var transaction = _appDbContext.Database.BeginTransaction();
            {
                try
                {
                    string userId = GetUserId();
                    var user = _appDbContext.Users.FirstOrDefault(x => x.Id == userId);
                    if (string.IsNullOrEmpty(userId))                   
                        throw new Exception("Usuario no conectado");
                    Order cart = GetItem(userId);
                    if (cart is null)
                        throw new Exception("No tiene Carrito");
                    var shoppingCart = _appDbContext.OrderItem
                        .Where(a => a.OrderId == cart.OrderId).ToList();
                    if (shoppingCart.Count == 0)
                        throw new Exception("carrito vacio");
                    double total = shoppingCart.Sum(item => item.UnitPrice * item.Quantity);
                    Order order = new Order
                    {
                        Id = userId,
                        CreateDate = DateTime.UtcNow,
                        StateId = 1, //pendiente
                        Name = user.Name,
                        Address = user.Address,
                        PhoneNumber = user.PhoneNumber,
                        Shipping = shippingDetails,
                        Comments = comments,
                        Total = total,
                    };
                    _appDbContext.Add(order);
                    _appDbContext.SaveChanges();
                    foreach(var item in shoppingCart) 
                    {
                        var orderCart = new OrderItem
                        {
                            ProductId = item.ProductId,
                            OrderId = order.OrderId,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                        };
                        _appDbContext.OrderItem.Add(orderCart);
                    }
                    _appDbContext.SaveChanges();

                    // removiendo el carrito
                    _appDbContext.OrderItem.RemoveRange(shoppingCart);
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
        }// ToDo

      
    }
}