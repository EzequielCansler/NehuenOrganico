using NehuenOrganico.Models;

namespace NehuenOrganico.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        void CreateOrder(Product product);
    }
}
