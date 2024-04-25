using NehuenOrganico.Models;

namespace NehuenOrganico.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        int AddItem(int ProductId, int qty);
        int RemoveItem(int ProductId);
        Order GetItem(string userId);
        string GetUserId();
        int GetCartItemCount(string userId="");
        List<OrderItem> GetUserCart();
        bool DoCheckOut(string shippingDetails, string comments);
    }
}