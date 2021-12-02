using System.Collections.Generic;
using ShopApp.Business.Abstract;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entity;

namespace ShopApp.Business.Concrete
{
    public class CategoryManager : ICategoryService
    {

        private IUnitOfWork _unitofwork;
        public CategoryManager(IUnitOfWork unitofwork)
        {
            this._unitofwork = unitofwork;
        }

        public bool Create(Category entity)
        {
            if (Validation(entity))
            {
                _unitofwork.Categories.Create(entity);
                _unitofwork.Save();
                return true;
            }
            return false;
        }

        public void Delete(Category entity)
        {
            _unitofwork.Categories.Delete(entity);
            _unitofwork.Save();
        }

        public void DeleteFromCategory(int productId, int categoryId)
        {
            _unitofwork.Categories.DeleteFromCategory(productId,categoryId);
            _unitofwork.Save();
        }

        public List<Category> GetAll()
        {
            return _unitofwork.Categories.GetAll();
        }

        public Category GetById(int id)
        {
            return _unitofwork.Categories.GetById(id);
        }

        public Category GetByIdWithProducts(int categoryId)
        {
            return _unitofwork.Categories.GetByIdWithProducts(categoryId);
        }

        public bool Update(Category entity)
        {
            if (Validation(entity))
            {
                _unitofwork.Categories.Update(entity);
                _unitofwork.Save();
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