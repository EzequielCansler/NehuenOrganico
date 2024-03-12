using Microsoft.AspNetCore.Mvc.ActionConstraints;
using NehuenOrganico.Data;
using NehuenOrganico.Models;
using System.Collections.Generic;
using System.Linq;

namespace NehuenOrganico.Repositories
{
    public class ProductsRepository
    {
        private static List<Product> _products = new()
        {
            new Product{ ProductId = 1 , Name = " Bolsón de 5/6 kg de verduras agroecológicas",Category="Verduras", Description  = "fede trolo",  Price = 3420 , ImageUrl="https://lh7-us.googleusercontent.com/IchrnWBkY7MUuuLGNEt2YkxXmmLBc6TblCE_QaBFIkL_RdDgNzZBNKQw0MLEuR57gTcEiimZPD0Fev9B-msQYxLMqOc446_RQ4gAfEr2KjM39zg3M8sy-xRNgjjuxHtTWDzwGUY7bQjxrCQApnh_bYxPM8pjtw"},
            new Product{ ProductId = 2 , Name = "Frutilla La plata 500grs ",Category="Frutas", Description  = "hay que mejorar la descripcion",  Price = 650 , ImageUrl="https://th.bing.com/th/id/R.f21513fe58e0f9bbdfb5bbc32cbba7b9?rik=2CzQ3LhxDN5qnw&riu=http%3a%2f%2fnwdistrict.ifas.ufl.edu%2ffcs%2ffiles%2f2020%2f03%2f022453-scaled.jpg&ehk=2KfQ1T4x%2fUM6Y%2fXgrvcLUJFobiTP6chmwW51B2fpknk%3d&risl=&pid=ImgRaw&r=0"},
            new Product{ ProductId = 3 , Name = "Espinaca ",Category="Verduras", Description  = "",  Price = 280 , ImageUrl="https://cdn2.salud180.com/sites/default/files/field/image/2010/12/espinaca5.jpg"}
        };

        public static void AddProduct(Product product)
        {
            var maxId = _products.Max(x => x.ProductId);
            product.ProductId = maxId++;
            _products.Add(product);
        }

        public static List<Product> GetProducts() => _products;

        public static Product? GetProductById(int ProductId)
        {
            var product = _products.FirstOrDefault(x => x.ProductId == ProductId);
            if (product != null)
            {
                return new Product
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Category = product.Category,
                    Description = product.Description,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl
                };
            }
            return null;
        }

        public static void UpdateProduct(Product updatedProduct)
        {
            using (var dbContext = new AppDbContext())
            {
                var existingProduct = dbContext.Products.FirstOrDefault(p => p.ProductId == updatedProduct.ProductId);
                if (existingProduct != null)
                {
                    existingProduct.Name = updatedProduct.Name;
                    existingProduct.Category = updatedProduct.Category;
                    existingProduct.Description = updatedProduct.Description;
                    existingProduct.Price = updatedProduct.Price;
                    existingProduct.ImageUrl = updatedProduct.ImageUrl;

                    dbContext.SaveChanges();
                }
            }
        }

        public static void DeleteProduct(int ProductId)
        {
            var product = _products.FirstOrDefault(x => x.ProductId == ProductId);
            if (product != null)
            {
                _products.Remove(product);
            }
        }
    }
}
