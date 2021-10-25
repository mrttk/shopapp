using System.Collections.Generic;
using ShopApp.Entity;

namespace ShopApp.DataAccess.Abstract
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Category GetByIdWithProducts(int categoryId); 
    }
}