﻿using NehuenOrganico.Models;
using NehuenOrganico.Repositories.Interfaces;


namespace NehuenOrganico.Services
{
    public class OrderService 
    {
        private readonly IItemRepository _itemRepo;
        private readonly IOrderRepository _orderRepo;


        public OrderService(IItemRepository itemRepo, IOrderRepository orderRepo)
        {
            _itemRepo = itemRepo;
            _orderRepo = orderRepo;
        }

        public bool CreateOrder(int orderId, string shippingDetails, string comments)
        {
            Order order = new Order();
            DateTime date = DateTime.Now;
            Double totalPrice = 0;
            List<OrderItem> items = _itemRepo.GetAllItemsFormID(orderId);
            foreach (OrderItem item in items)
            {
                totalPrice += item.UnitPrice * item.Quantity;
            }

            bool result = _orderRepo.CreateOrder(orderId, shippingDetails,comments, totalPrice, date);

            return result;
        }
    }
}
