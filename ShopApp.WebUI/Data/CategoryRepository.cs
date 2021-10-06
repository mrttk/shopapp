using System.Collections.Generic;
using System.Linq;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.Data
{
    public class CategoryRepository
    {
        private static List<Category> _categories = null;
        static CategoryRepository()
        {
            _categories = new List<Category>
            {
                new Category {CategoryId=1, Name="Phones", Description="Phone Categories"},
                new Category {CategoryId=2, Name="Electronic", Description="Electronic  Categories"},
                new Category {CategoryId=3, Name="Computer", Description="Computer Categories"}
            };
        }

        public static List<Category> Categories
        {
            get
            {
                return _categories;
            }
        }

        public static void AddCategory (Category category)
        {
            _categories.Add(category);
        }

        public static Category GetCategoryById(int id)
        {
            return _categories.FirstOrDefault(c=>c.CategoryId==id);
        }
    }
}