using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopApp.WebUI.Data;
using ShopApp.WebUI.Models;
using ShopApp.WebUI.ViewModels;

namespace ShopApp.WebUI.Controllers
{
    public class ProductController: Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult List(int? id, string q)
        {
            var products = ProductRepository.Products;
            if (id!=null)
            {
                products = products.Where(p=>p.CategoryId == id).ToList();
            }

            if (!string.IsNullOrEmpty(q))
            {
                products = products.Where(i=>i.Name.ToLower().Contains(q.ToLower())).ToList();
            }

            var productViewModel = new ProductViewModel()
            {
                Products = products
            };

            return View(productViewModel);
        }
        
        public IActionResult Details(int id)
        {
            return View(ProductRepository.GetProductById(id));
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(CategoryRepository.Categories,"CategoryId","Name");
            return View(new Product());
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                ProductRepository.AddProduct(product);
                return RedirectToAction("List");
            }
            ViewBag.Categories = new SelectList(CategoryRepository.Categories,"CategoryId","Name");
            return View(product);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Categories = new SelectList(CategoryRepository.Categories,"CategoryId","Name");
            return View(ProductRepository.GetProductById(id));
        }

        [HttpPost]
        public IActionResult Edit(Product p)
        {
            ProductRepository.EditProduct(p);
            return RedirectToAction("list");
        }
    }
}