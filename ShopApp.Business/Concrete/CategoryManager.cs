using System.Collections.Generic;
using ShopApp.Business.Abstract;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entity;

namespace ShopApp.Business.Concrete
{
    public class CategoryManager : ICategoryService
    {

        private ICategoryRepository _categoryRepository;
        public CategoryManager(ICategoryRepository categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }        
        public void Create(Category entity)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(Category entity)
        {
            throw new System.NotImplementedException();
        }

        public List<Category> GetAll()
        {
            return _categoryRepository.GetAll();
        }

        public Category GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Category entity)
        {
            throw new System.NotImplementedException();
        }
    }
}