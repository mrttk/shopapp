using System.Collections.Generic;
using ShopApp.Entity;

namespace ShopApp.DataAccess.Abstract
{
    public interface IProductRepository : IRepository<Product>
    {
        Product GetProductDetails(string url);
        List<Product> GetHomePageProducts();
        List<Product> GetSearchResult(string searhString);
        List<Product> GetProductsByCategory(string name, int page, int pageSize);
        int GetCountByCategory(string category);
    }
}