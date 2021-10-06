using System.Collections.Generic;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entity;

namespace ShopApp.DataAccess.Concrete.EFCore
{
    public class EFCoreProductRepository : IProductRepository
    {
        private ShopContext _db = new ShopContext();
        public void Create(Product entity)
        {
            _db.Products.Add(entity);
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public List<Product> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Product GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public List<Product> GetPopularProducts()
        {
            throw new System.NotImplementedException();
        }

        public void Update(Product entity)
        {
            throw new System.NotImplementedException();
        }
    }
}