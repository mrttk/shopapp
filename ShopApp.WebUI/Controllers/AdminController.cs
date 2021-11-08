using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopApp.Business.Abstract;
using ShopApp.Entity;
using ShopApp.WebUI.Extensions;
using ShopApp.WebUI.Identity;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.Controllers
{
    [Authorize]//(Roles = "Admin")
    public class AdminController : Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;

        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;
        public AdminController(IProductService productService, ICategoryService categoryService, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            this._productService = productService;
            this._categoryService = categoryService;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public IActionResult RoleList()
        {
            return View(_roleManager.Roles);
        }

        public IActionResult RoleCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleModel model)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));
            if (result.Succeeded)
            {
                return RedirectToAction("RoleList");
            }
            else   
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> RoleEdit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            var members = new List<User>();
            var nonMembers = new List<User>();
            
            foreach (var user in _userManager.Users)
            {
                var list = await _userManager.IsInRoleAsync(user,role.Name)
                                ? members : nonMembers;
                list.Add(user);
            }

            var model = new RoleDetails(){
                Role = role,
                Members = members,
                NonMembers = nonMembers
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult>RoleEdit(RoleEditModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var userId in model.IdsToAdd ?? new string[]{})
                {
                    var user = await _userManager.FindByIdAsync(userId);

                    if (user != null)
                    {
                        var result = await _userManager.AddToRoleAsync(user,model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("",error.Description);
                            }
                        }    
                    }
                }

                foreach (var userId in model.IdsToDelete ?? new string[]{})
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user,model.RoleName);

                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("",error.Description);
                            }
                        }
                    }
                }
                
                return Redirect("/admin/role/"+model.RoleId);

            }
            return View(model);
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
        public async Task<IActionResult> ProductCreate(ProductModel model, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var entity = new Product()
                {
                    Name = model.Name,
                    Url = model.Url,
                    Price = (double)model.Price,
                    Description = model.Description
                };

                if (file != null)
                {
                    var extention = Path.GetExtension(file.FileName);
                    var fileName = string.Format($"{entity.Name}-{DateTime.Now.ToString("dd'-'MM'-'yyyy'-'HH'-'mm'-'ss")}{extention}");
                    entity.ImageUrl = fileName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\images",fileName);

                    using (var stream = new FileStream(path,FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                if (_productService.Create(entity))
                {
                    TempData.Put("message", new AlertMessage(){
                        Title = "Create Message",
                        Message = "Record added!",
                        AlertType = "success"
                    });
                    return RedirectToAction("ProductList");
                } 
                TempData.Put("message", new AlertMessage(){
                        Title = "Error Message",
                        Message = _productService.ErrorMessage,
                        AlertType = "danger"
                });           
                return View(model);
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
        public async Task<IActionResult> ProductEdit(ProductModel model, int[] categoryIds,IFormFile file)
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
                entity.IsApproved = model.IsApproved;
                entity.IsHome = model.IsHome;

                if (file!=null)
                {
                    var extention = Path.GetExtension(file.FileName);
                    var randomName = string.Format($"{entity.Name}-{DateTime.Now.ToString("dd'-'MM'-'yyyy'-'HH'-'mm'-'ss")}{extention}");
                    entity.ImageUrl = randomName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\images",randomName);

                    using (var stream = new FileStream(path,FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                if (_productService.Update(entity, categoryIds))
                {
                    TempData.Put("message", new AlertMessage(){
                        Title = "Update Message",
                        Message = "Record updated!",
                        AlertType = "success"
                    });
                    return RedirectToAction("ProductList");
                }
                ViewBag.Categories = _categoryService.GetAll();
                TempData.Put("message", new AlertMessage(){
                    Title = "Error Message",
                    Message = _productService.ErrorMessage,
                    AlertType = "danger"
                });
                return View(model);
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
            
            TempData.Put("message", new AlertMessage(){
                Title = "Delete Message",
                Message = $"{entity.Name} deleted!",
                AlertType = "danger"
            });

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

                if (_categoryService.Create(entity))
                {
                    TempData.Put("message", new AlertMessage(){
                        Title = "Create Message",
                        Message = "Category created!",
                        AlertType = "success"
                    });
                    return RedirectToAction("CategoryList");
                }
                TempData.Put("message", new AlertMessage(){
                    Title = "Error Message",
                    Message = _categoryService.ErrorMessage,
                    AlertType = "danger"
                });
                return View(model);
                
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

                if (_categoryService.Update(entity))
                {
                    TempData.Put("message", new AlertMessage(){
                        Title = "Update Message",
                        Message = "Category updated!",
                        AlertType = "success"
                    });
                    return RedirectToAction("CategoryList");     
                }
                TempData.Put("message", new AlertMessage(){
                    Title = "Error Message",
                    Message = _categoryService.ErrorMessage,
                    AlertType = "danger"
                });
                return View(model);
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
            
            TempData.Put("message", new AlertMessage(){
                Title = "Delete Message",
                Message = $"{entity.Name} deleted!",
                AlertType = "danger"
            });

            return RedirectToAction("CategoryList");
        }

        public IActionResult DeleteFromCategory(int productId,int categoryId)
        {
            _categoryService.DeleteFromCategory(productId,categoryId);
            return Redirect("/admin/categories/"+categoryId);
        }

    }

    
}