using System.Collections.Generic;
using ShopApp.Entity;

namespace ShopApp.Business.Abstract
{
    public interface ICategoryService : IValidator<Category>
    {
        Category GetById(int id);
        List<Category> GetAll();
        Category GetByIdWithProducts(int categoryId);
        void DeleteFromCategory(int productId,int categoryId);
        bool Create(Category entity);
        bool Update(Category entity);
        void Delete(Category entity);
    }
}