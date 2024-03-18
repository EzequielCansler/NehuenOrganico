using NehuenOrganico.Models;

namespace NehuenOrganico.Repositories.Interfaces
{
    public interface IProductRepository
    {
        List<Product> GetAll();
        Product? GetById(int id);
        void Update(Product product);
        Product AddProduct(Product product);
        void DeleteProduct(int id);
    }
}
