using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.Entity;
using ShopApp.WebUI.Models;
using ShopApp.WebUI.ViewModels;

namespace ShopApp.WebUI.Controllers
{
    public class ShopController : Controller
    {
        private IProductService _productService;

        public ShopController(IProductService productService)
        {
            this._productService = productService;
        }

        public IActionResult List(string category, int page = 1)
        {
            const int pageSize = 1;
            var productListViewModel = new ProductListViewModel()
            {
                PageInfo = new PageInfo(){
                   TotalItems = _productService.GetCountByCategory(category),
                   CurrentPage = page,
                   CurrentCategory = category,
                   ItemsPerPage = pageSize
                },
                Products = _productService.GetProductsByCategory(category, page , pageSize)
            };
            return View(productListViewModel);
        }

        public IActionResult Details(string url)
        {
            if (url == null)
            {
                return NotFound();
            }
            Product product = _productService.GetProductDetails(url);

            if (product == null)
            {
                return NotFound();
            }
            return View(new ProductDetailModel{
                Product = product,
                Categories = product.ProductCategories.Select(i=>i.Category).ToList()
            });
        }

    }
}