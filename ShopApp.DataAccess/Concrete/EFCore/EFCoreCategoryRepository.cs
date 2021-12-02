using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entity;

namespace ShopApp.DataAccess.Concrete.EFCore
{ 
    public class EFCoreCategoryRepository : EFCoreGenericRepository<Category>, ICategoryRepository
    {
        public EFCoreCategoryRepository(ShopContext context) : base(context)
        {
            
        }
        private ShopContext ShopContext
        {
            get { return context as ShopContext; }
        }
        public void DeleteFromCategory(int productId, int categoryId)
        {
            var cmd = "delete from productcategory where productId=@p0 and categoryId=@p1";
            ShopContext.Database.ExecuteSqlRaw(cmd,productId,categoryId);
        }

        public Category GetByIdWithProducts(int categoryId)
        {
            return ShopContext.Categories
                                .Where(i=>i.CategoryId==categoryId)
                                .Include(i=>i.ProductCategories)
                                .ThenInclude(i=>i.Product)
                                .FirstOrDefault();
        }
    }
}