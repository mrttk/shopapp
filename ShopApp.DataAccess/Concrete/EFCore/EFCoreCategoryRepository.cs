using System.Collections.Generic;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entity;

namespace ShopApp.DataAccess.Concrete.EFCore
{ 
    public class EFCoreCategoryRepository : EFCoreGenericRepository<Category, ShopContext>, ICategoryRepository
    {
        public List<Category> GetPopularCategories()
        {
            throw new System.NotImplementedException();
        }
    }
}