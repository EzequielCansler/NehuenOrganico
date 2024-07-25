using NehuenOrganico.Models;

namespace NehuenOrganico.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        int AddItem(int ProductId, int qty);
        int RemoveItem(int ProductId);
        Order GetOrder(string userId);
        string GetUserId();
        int GetCartItemCount(int orderId);
        List<OrderItem> GetUserCart();
        bool CreateOrder(int id,string shippingDetails, string comments, double total,DateTime date);
        List<Order> GetUserOrders();
        public int? GetStateIdByOrderId(int? orderId);

    }
}