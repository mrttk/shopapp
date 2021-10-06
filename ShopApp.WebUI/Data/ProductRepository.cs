using System.Collections.Generic;
using ShopApp.WebUI.Models;
using System.Linq;

namespace ShopApp.WebUI.Data
{
    public static class ProductRepository
    {
        private static List<Product> _products = null;
        static ProductRepository()
        {
            _products = new List<Product>
            {
                new Product { ProductId = 1, Name="IPhone7", Price=2000, Description="Nice phone", ImageUrl = "1.jpg", CategoryId = 1},
                new Product { ProductId = 2, Name="IPhone8", Price=3000, Description="Good phone", ImageUrl = "2.jpg", CategoryId = 1},
                new Product { ProductId = 3, Name="IPhoneX", Price=6000, Description="Better than IPhone8", ImageUrl = "3.jpg", CategoryId = 1},
                new Product { ProductId = 4, Name="Lenovo 6", Price=2000, Description="Nice laptop", ImageUrl = "4.jpg", CategoryId = 3},
                new Product { ProductId = 5, Name="Lenovo 8", Price=3000, Description="Good laptop", ImageUrl = "5.jpg", CategoryId = 3},
                new Product { ProductId = 6, Name="Lenovo 9", Price=6000, Description="Better than others", ImageUrl = "6.jpg", CategoryId = 3}
            };
        }

        public static List<Product> Products
        {
            get
            {
                return _products;
            }
        }

        public static void AddProduct (Product product)
        {
            _products.Add(product);
        }

        public static Product GetProductById (int id)
        {
            return _products.FirstOrDefault(p=>p.ProductId==id);
        }

        public static void EditProduct(Product product){
            foreach (var p in _products)
            {
                if (p.ProductId==product.ProductId)
                {
                    p.Name = product.Name;
                    p.Price = product.Price;
                    p.Description = product.Description;
                    p.ImageUrl = product.ImageUrl;
                    p.IsApproved = product.IsApproved;
                    p.CategoryId = product.CategoryId;
                }
            }
        }

        public static void DeleteProduct(int ProductId)
        {
            var product = ProductRepository.GetProductById(ProductId);
            if (product!=null)
            {
                _products.Remove(product);
            }
        }
    }
}