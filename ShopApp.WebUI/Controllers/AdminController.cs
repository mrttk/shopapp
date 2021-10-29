using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopApp.Business.Abstract;
using ShopApp.Entity;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;
        public AdminController(IProductService productService, ICategoryService categoryService)
        {
            this._productService = productService;
            this._categoryService = categoryService;
        }

        public IActionResult ProductList()
        {
            return View(new ProductListViewModel(){
                Products = _productService.GetAll()
            });
        }

        public IActionResult CategoryList()
        {
            return View( new CategoryListViewModel(){
                Categories = _categoryService.GetAll()
            });
        }
        

        [HttpGet]
        public IActionResult ProductCreate()
        {

            return View();
        }

        [HttpPost]
        public IActionResult ProductCreate(ProductModel model)
        {
            if (ModelState.IsValid)
            {
            var entity = new Product(){
                Name = model.Name,
                Url = model.Url,
                ImageUrl = model.ImageUrl,
                Price = (double)model.Price,
                Description = model.Description

            };

            _productService.Create(entity);

            var info = new AlertMessage(){
                Message = $"{entity.Name} added!",
                AlertType = "success"
            };

            TempData["message"] = JsonConvert.SerializeObject(info);
            
            return RedirectToAction("ProductList");
                
            }

            return View(model);
        }
        

        [HttpGet]
        public IActionResult ProductEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = _productService.GetByIdWithCategories((int)id);

            if (entity == null)
            {
                return NotFound();
            }

            var model = new ProductModel(){
                ProductId = entity.ProductId,
                Name = entity.Name,
                Url = entity.Url,
                Price = entity.Price,
                ImageUrl = entity.ImageUrl,
                Description = entity.Description,
                IsApproved = entity.IsApproved,
                IsHome = entity.IsHome,
                SelectedCategories = entity.ProductCategories.Select(i=>i.Category).ToList()
            };

            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }
        
        [HttpPost]
        public IActionResult ProductEdit(ProductModel model, int[] categoryIds)
        {
            if (ModelState.IsValid)
            {
                var entity = _productService.GetById(model.ProductId);
                if (entity == null)
                {
                    return NotFound();
                }
                entity.Name = model.Name;
                entity.Url = model.Url;
                entity.Price = (double)model.Price;
                entity.Description = model.Description;
                entity.ImageUrl = model.ImageUrl;
                entity.IsApproved = model.IsApproved;
                entity.IsHome = model.IsHome;

                _productService.Update(entity, categoryIds);
                
                var info = new AlertMessage(){
                    Message = $"{entity.Name} updated!",
                    AlertType = "success"
                };

                TempData["message"] = JsonConvert.SerializeObject(info);

                return RedirectToAction("ProductList");
            }
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }

        public IActionResult ProductDelete(int productId)
        {
            var entity = _productService.GetById(productId);
            if (entity != null)
            {
                _productService.Delete(entity);
            }

            var info = new AlertMessage(){
                Message = $"{entity.Name} deleted!",
                AlertType = "danger"
            };

            TempData["message"] = JsonConvert.SerializeObject(info);

            return RedirectToAction("ProductList");
        }

        [HttpGet]
        public IActionResult CategoryCreate()
        {

            return View();
        }

        [HttpPost]
        public IActionResult CategoryCreate(CategoryModel model)
        {
            if (ModelState.IsValid)
            {                
                var entity = new Category(){
                    Name = model.Name,
                    Url = model.Url
                };

                _categoryService.Create(entity);

                var info = new AlertMessage(){
                    Message = $"{entity.Name} added!",
                    AlertType = "success"
                };

                TempData["message"] = JsonConvert.SerializeObject(info);
                
                return RedirectToAction("CategoryList");
            }
            return View(model);
        }
        

        [HttpGet]
        public IActionResult CategoryEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = _categoryService.GetByIdWithProducts((int)id);

            if (entity == null)
            {
                return NotFound();
            }

            var model = new CategoryModel(){
                CategoryId = entity.CategoryId,
                Name = entity.Name,
                Url = entity.Url,
                Products = entity.ProductCategories.Select(p=>p.Product).ToList()
            };
            return View(model);
        }
        
        [HttpPost]
        public IActionResult CategoryEdit(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _categoryService.GetById(model.CategoryId);
                if (entity == null)
                {
                    return NotFound();
                }
                entity.Name = model.Name;
                entity.Url = model.Url;

                _categoryService.Update(entity);
                
                var info = new AlertMessage(){
                    Message = $"{entity.Name} updated!",
                    AlertType = "success"
                };

                TempData["message"] = JsonConvert.SerializeObject(info);

                return RedirectToAction("CategoryList");                
            }
            return View(model);
        }

        public IActionResult DeleteCategory(int categoryId)
        {
            var entity = _categoryService.GetById(categoryId);
            if (entity != null)
            {
                _categoryService.Delete(entity);
            }

            var info = new AlertMessage(){
                Message = $"{entity.Name} deleted!",
                AlertType = "danger"
            };

            TempData["message"] = JsonConvert.SerializeObject(info);

            return RedirectToAction("CategoryList");
        }

        public IActionResult DeleteFromCategory(int productId,int categoryId)
        {
            _categoryService.DeleteFromCategory(productId,categoryId);
            return Redirect("/admin/categories/"+categoryId);
        }
        

    }

    
}