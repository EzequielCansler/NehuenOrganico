using NehuenOrganico.Data;
using NehuenOrganico.Models;
using Microsoft.EntityFrameworkCore;
using NehuenOrganico.Repositories.Interfaces;

namespace NehuenOrganico.Repositories
{
    public class ProductsRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductsRepository(AppDbContext context)
        {
            _context = context;
        }

        public  List<Product> GetAll()
        {
            return  _context.Product.ToList();
        }


        public Product? GetById(int ProductId)
        {
            var product = _context.Product.FirstOrDefault(x => x.ProductId == ProductId);
            if (product != null)
            {
                return new Product
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    CategoryId = product.CategoryId,          
                    Price = product.Price,
                    ImageUrl = product.ImageUrl
                };
            }
            return null;
        }
       

        public void Update(Product updatedProduct)
        {           
                var existingProduct = _context.Product.FirstOrDefault(p => p.ProductId == updatedProduct.ProductId);
                if (existingProduct != null)
                {
                    existingProduct.Name = updatedProduct.Name;
                    existingProduct.CategoryId = updatedProduct.CategoryId;                  
                    existingProduct.Price = updatedProduct.Price;
                    existingProduct.ImageUrl = updatedProduct.ImageUrl;

                    _context.SaveChanges();
                }         
        }

        public Product AddProduct(Product product)
        {
            var addProduct = new Product   
            {
                Name = product.Name,
                CategoryId = product.CategoryId,             
                Price = product.Price,
                ImageUrl = product.ImageUrl
            };
            _context.Product.Add(addProduct);
            _context.SaveChanges();
            return (addProduct);
        }


        public void DeleteProduct(int productId)
        {
            Product product = _context.Product.FirstOrDefault(x => x.ProductId == productId);
            if (product != null)
            {
                _context.Product.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}
