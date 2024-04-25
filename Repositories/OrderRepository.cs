using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NehuenOrganico.Data;
using NehuenOrganico.Models;
using NehuenOrganico.Repositories.Interfaces;
using System.Linq;

namespace NehuenOrganico.Repositories
{
    public class OrderRepository
    {
        
        private readonly AppDbContext _appDbContext;
        private readonly IOrderRepository _orderRepo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public OrderRepository(AppDbContext appDbContext, IOrderRepository orderRepo,UserManager<IdentityUser> userManager, IHttpContextAccessor contextAccessor)
        {
            _appDbContext = appDbContext;
            _contextAccessor = contextAccessor;
            _orderRepo = orderRepo; 
            _userManager = userManager;
        }


        public int AddItem(int ProductId,int qty)
        {
            string userId = GetUserId();
            using var transaction = _appDbContext.Database.BeginTransaction();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("Usuario no conectado");
                Order cart = GetItem(userId);
                if(cart == null)
                {
                    cart = new Order
                    {
                        Id = userId,
                    };
                    _appDbContext.Add(cart);
                }
                // order item
                var cartItem = _appDbContext.OrderItem.FirstOrDefault(x => x.OrderId == cart.OrderId && x.ProductId == ProductId);
                if(cartItem != null)
                {
                    cartItem.Quantity += qty;
                }
                else
                {
                    Product produ = _appDbContext.Product.Find(ProductId);
                    cartItem = new OrderItem
                    {
                        ProductId = ProductId,
                        OrderId = cartItem.OrderId,
                        Quantity = qty,
                        UnitPrice = produ.Price
                    };
                    _appDbContext.OrderItem.Add(cartItem);
                }
                _appDbContext.SaveChanges();
                transaction.Commit();               
            }
            catch(Exception ex)
            {
            }
            var cartItemCount = GetCartItemCount(userId);
            return cartItemCount;
        } // done
        public int RemoveItem(int ProductId)
        {            
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("Usuario no conectado");
                Order cart = GetItem(userId);
                if (cart == null)
                    throw new Exception("Carrito invalido");
                // order item
                var cartItem = _appDbContext.OrderItem.FirstOrDefault(x => x.OrderId == cart.OrderId && x.ProductId == ProductId);
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
        private Order GetItem(string userId) 
        {
            Order item = _appDbContext.Order.FirstOrDefault(x => x.Id == userId);
            return item;
        }// done
        private string GetUserId()
        {
            var user = _contextAccessor.HttpContext.User;
            var userId = _userManager.GetUserId(user);
            return userId;
        } // done
        private int GetCartItemCount(string userId="")
        {
            if (!string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            Order order = _appDbContext.Order.FirstOrDefault(x => x.Id == userId);
            return _appDbContext.OrderItem.Count(x => x.OrderId == order.OrderId);
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
                    return false;
                }            
            }
        }// ToDo

    }
}