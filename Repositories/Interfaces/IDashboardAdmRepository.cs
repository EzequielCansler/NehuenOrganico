using NehuenOrganico.Models;

namespace NehuenOrganico.Repositories.Interfaces
{
    public interface IDashboardAdmRepository
    {
        List<Order> GetAllOrderWithStatePending();
        List<dynamic> GetAllProductsWithStatePending();

    }
}
