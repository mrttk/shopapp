using System.Collections.Generic;
using ShopApp.Entity;

namespace ShopApp.Business.Abstract
{
    public interface ICategoryService
    {
        Category GetById(int id);
        List<Category> GetAll();
        Category GetByIdWithProducts(int categoryId);
        void Create(Category entity);
        void Update(Category entity);
        void Delete(Category entity);
    }
}