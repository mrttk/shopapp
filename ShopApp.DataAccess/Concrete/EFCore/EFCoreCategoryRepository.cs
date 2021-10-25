using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entity;

namespace ShopApp.DataAccess.Concrete.EFCore
{ 
    public class EFCoreCategoryRepository : EFCoreGenericRepository<Category, ShopContext>, ICategoryRepository
    {
        public Category GetByIdWithProducts(int categoryId)
        {
            using (var context = new ShopContext())
            {
                return context.Categories
                                    .Where(i=>i.CategoryId==categoryId)
                                    .Include(i=>i.ProductCategories)
                                    .ThenInclude(i=>i.Product)
                                    .FirstOrDefault();
            }
        }
    }
}