using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
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

        public IActionResult List()
        {
            var productListViewModel = new ProductListViewModel()
            {
                Products = _productService.GetAll()
            };
            return View(productListViewModel);
        }
    
    }
}