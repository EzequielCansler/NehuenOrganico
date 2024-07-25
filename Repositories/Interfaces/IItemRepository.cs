using NehuenOrganico.Models;

namespace NehuenOrganico.Repositories.Interfaces
{
    public interface IItemRepository
    {
        List<OrderItem> GetAllItemsFormID(int id);
        int GetItemCount();
        List<OrderItem> GetAllItems();

    }
}
