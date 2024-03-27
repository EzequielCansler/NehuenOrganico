using Microsoft.AspNetCore.Mvc;
using NehuenOrganico.Data;
using NehuenOrganico.Models;
using NehuenOrganico.Repositories.Interfaces;

namespace NehuenOrganico.Repositories
{
    public class OrderRepository
    {
        
        private readonly AppDbContext _appDbContext;
        private readonly IProductRepository _productRepo;
        private readonly IOrderRepository _orderRepo;
        public OrderRepository(AppDbContext appDbContext, IProductRepository productRepo, IOrderRepository orderRepo)
        {
            _appDbContext = appDbContext;
            _productRepo = productRepo;
            _orderRepo = orderRepo; 
        }

       
        public Order GetOrderById(int? id)
        {
            Order order = _appDbContext.Order.FirstOrDefault(p => p.OrderId == id);
            if (order != null)
            {
                return new Order
                {
                    OrderId = order.OrderId,
                    Name = order.Name,
                    Address = order.Address,
                    Shipping = order.Shipping,
                    Comments = order.Comments,
                    Total = order.Total,
                    Id = order.Id,
                    PhoneNumber = order.PhoneNumber,
                    State = order.State,
                    UserIp = order.UserIp,
                };
            }
            else
            {
                return null;
            }       
        }
        public void UpdateOrder(Order updateOrder)
        {
            Order existingOrder = _appDbContext.Order.FirstOrDefault(o => o.OrderId == updateOrder.OrderId);
            if (existingOrder != null)
            {
                existingOrder.Name = updateOrder.Name;
                existingOrder.PhoneNumber = updateOrder.PhoneNumber;
                existingOrder.Address = updateOrder.Address;
                existingOrder.Comments = updateOrder.Comments;
                existingOrder.Shipping = updateOrder.Shipping;
                existingOrder.Total = updateOrder.Total;
                _appDbContext.SaveChanges();

            }
        }
        public List<OrderItem> GetAllOrderItems()
        {
            return _appDbContext.OrderItem.ToList();
             
        }
        public 
        public Order CreateOrder(Order newOrder, List<OrderItem> orderItem)
        {
            

            Order addOrder = new Order
            {
                Name = newOrder.Name,
                Address = newOrder.Address,
                Shipping = newOrder.Shipping,
                Comments = newOrder.Comments,
                Total = newOrder.Total,
                Id = newOrder.Id,
                PhoneNumber = newOrder.PhoneNumber,
                State = newOrder.State,
                UserIp = newOrder.UserIp,
            };
            _appDbContext.Order.Add(addOrder);
            _appDbContext.SaveChanges();
                return newOrder;
        }

    }



}
