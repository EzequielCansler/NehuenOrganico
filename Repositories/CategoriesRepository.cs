using NehuenOrganico.Data;
using NehuenOrganico.Repositories.Interfaces;
using NehuenOrganico.Models;

namespace NehuenOrganico.Repositories
{
    public class CategoriesRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        
        public CategoriesRepository(AppDbContext context)
        {
            _context = context;
            
        }

        public List<Categories> GetAllCategories()
        {
           return _context.Categories.ToList();
        }
    }
}
