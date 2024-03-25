using NehuenOrganico.Models;

namespace NehuenOrganico.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
       List<Category> GetAllCategories();
    }
}
