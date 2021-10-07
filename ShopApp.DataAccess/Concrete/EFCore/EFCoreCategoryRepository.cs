using System.Collections.Generic;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entity;

namespace ShopApp.DataAccess.Concrete.EFCore
{
    public class EFCoreCategoryRepository : ICategoryRepository
    {
        private ShopContext _db = new ShopContext();
        public void Create(Category entity)
        {
            _db.Categories.Add(entity);
            _db.SaveChanges();
        }

        public void Delete(Category entity)
        {
            throw new System.NotImplementedException();
        }

        public List<Category> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Category GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public List<Category> GetPopularCategories()
        {
            throw new System.NotImplementedException();
        }

        public void Update(Category entity)
        {
            throw new System.NotImplementedException();
        }
    }
}