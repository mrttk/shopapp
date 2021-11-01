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

        public bool Create(Category entity)
        {
            if (Validation(entity))
            {
                _categoryRepository.Create(entity);
                return true;
            }
            return false;
        }

        public void Delete(Category entity)
        {
            _categoryRepository.Delete(entity);
        }

        public void DeleteFromCategory(int productId, int categoryId)
        {
            _categoryRepository.DeleteFromCategory(productId,categoryId);
        }

        public List<Category> GetAll()
        {
            return _categoryRepository.GetAll();
        }

        public Category GetById(int id)
        {
            return _categoryRepository.GetById(id);
        }

        public Category GetByIdWithProducts(int categoryId)
        {
            return _categoryRepository.GetByIdWithProducts(categoryId);
        }

        public bool Update(Category entity)
        {
            if (Validation(entity))
            {
                _categoryRepository.Update(entity);
                return true;                
            }
            return false;
        }

        public string ErrorMessage { get; set; }
        public bool Validation(Category entity)
        {
            var isValid = true;
            
            if (string.IsNullOrEmpty(entity.Name))
            {
                ErrorMessage += "Category name is required.\n";
                isValid = false;
            }

            if (string.IsNullOrEmpty(entity.Name) || entity.Name.Length < 2 || entity.Name.Length > 50)
            {
                ErrorMessage += "The category name must be a string with a minimum length of 2 and a maximum length of 50.\n";
                isValid = false;
            }

            if (string.IsNullOrEmpty(entity.Url))
            {
                ErrorMessage += "Category url is required.\n";
                isValid = false;
            }

            if (string.IsNullOrEmpty(entity.Url) || entity.Url.Length < 2 || entity.Url.Length > 50)
            {
                ErrorMessage += "The category url must be a string with a minimum length of 2 and a maximum length of 50.\n";
                isValid = false;
            }            
            return isValid;
        }
    }
}