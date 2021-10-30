using System.Collections.Generic;
using ShopApp.Business.Abstract;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entity;

namespace ShopApp.Business.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductRepository _productRepository;
        public ProductManager(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        public bool Create(Product entity)
        {
            if (Validation(entity))
            {
                _productRepository.Create(entity);
                return true;                
            }
            return false;
        }

        public void Delete(Product entity)
        {
            _productRepository.Delete(entity);
        }

        public List<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public Product GetById(int id)
        {
            return _productRepository.GetById(id);
        }

        public Product GetByIdWithCategories(int id)
        {
            return _productRepository.GetByIdWithCategories(id);
        }

        public int GetCountByCategory(string category)
        {
            return _productRepository.GetCountByCategory(category);
        }

        public List<Product> GetHomePageProducts()
        {
            return _productRepository.GetHomePageProducts();
        }

        public Product GetProductDetails(string url)
        {
            return _productRepository.GetProductDetails(url);
        }

        public List<Product> GetProductsByCategory(string name, int page, int pageSize)
        {
            return _productRepository.GetProductsByCategory(name, page, pageSize);
        }

        public List<Product> GetSearchResult(string searchString)
        {
            return _productRepository.GetSearchResult(searchString);
        }

        public void Update(Product entity)
        {
            _productRepository.Update(entity);
        }

        public bool Update(Product entity, int[] categoryIds)
        {
            if (Validation(entity))
            {
                _productRepository.Update(entity,categoryIds);
                return true;
            }
            return false;
        }

        public string ErrorMessage { get; set; }
        public bool Validation(Product entity)
        {
            var isValid = true;

            if (string.IsNullOrEmpty(entity.Name))
            {
                ErrorMessage += "Product name is required.\n";
                isValid = false;
            }

            if (string.IsNullOrEmpty(entity.Url))
            {
                ErrorMessage += "Product url is required.\n";
                isValid = false;
            }

            if (entity.Price < 1 || entity.Price > 100000) 
            {
                ErrorMessage += "Product price must be between 1 and 100000.\n";
                isValid = false;
            }

            if (string.IsNullOrEmpty(entity.Description) || entity.Description.Length < 5 || entity.Description.Length > 500)
            {
            ErrorMessage += "The description must be a string with a minimum length of 5 and a maximum length of 500.\n";
            isValid = false;
            }

            if (!entity.IsApproved && entity.IsHome)
            {
                ErrorMessage += "The product must be approved to add it to the homepage.\n";
                isValid = false;
            }

            return isValid;
        }
    }
}