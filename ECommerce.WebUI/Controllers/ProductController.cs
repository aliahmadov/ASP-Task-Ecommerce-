using App.Business.Abstract;
using App.Entities.Models;
using ECommerce.WebUI.ExtentionMethods;
using ECommerce.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace ECommerce.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;
        public ProductListViewModel Model { get; set; }
        public ProductController(IProductService productService, ICategoryService categoryService)
        {

            Model = new ProductListViewModel();
            _productService = productService;
            _categoryService = categoryService;

        }

        public IActionResult Index(int page = 1, int category = 0)
        {

            var products = _productService.GetAllByCategory(category);
            int pageSize = 10;
            Model.Products = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            Model.CurrentCategory = category;
            Model.PageCount = (int)Math.Ceiling(products.Count / (double)pageSize);
            Model.PageSize = pageSize;
            Model.CurrentPage = page;

            if (HttpContext.Session.GetString("model") != null)
            {

                string filter = HttpContext.Session.GetString("model");

                Model.NumFilter = "High to Low";
                Model.NameFilter = "Z to A";
                if (filter == "A to Z")
                {
                    Model.Products = Model.Products.OrderBy(p => p.ProductName).ToList();
                    Model.NameFilter = "Z to A";
                }
                else if (filter == "Z to A")
                {
                    Model.Products = Model.Products.OrderByDescending(p => p.ProductName).ToList();
                    Model.NameFilter = "A to Z";
                }

                if (filter == "low to high")
                {
                    Model.Products = Model.Products.OrderBy(p => p.UnitPrice).ToList();
                    Model.NumFilter = "High to Low";

                }
                else if (filter == "high to low")
                {
                    Model.Products = Model.Products.OrderByDescending(p => p.UnitPrice).ToList();
                    Model.NumFilter = "Low to High";
                }
            

                HttpContext.Session.SetString("model", filter);
                

            }

            else
            {
                Model.NumFilter = "Low to High";
                Model.NameFilter = "Z to A";
            }   

            HttpContext.Session.SetString("category", category.ToString());
            HttpContext.Session.SetString("page", page.ToString());

            return View(Model);
        }


        public IActionResult Sort(string value)
        {

            var filter = HttpContext.Session.GetString("model");
            var category = HttpContext.Session.GetString("category");
            var page = HttpContext.Session.GetString("page");

            if (filter == null)
            {
                if (value == "l")
                {
                    filter = "A to Z";
                    HttpContext.Session.SetString("model", "A to Z");
                }
                else
                {
                    filter = "low to high";
                    HttpContext.Session.SetString("model", "low to high");
                }
            }

            if (value == "l")
            {
                if (filter == "low to high" || filter == "high to low")
                {
                    filter = "A to Z";
                }
                if (filter == "A to Z")
                {
                    filter = "Z to A";

                }
                else if (filter == "Z to A")
                {
                    filter = "A to Z";
                }
            }

            if (value == "n")
            {
                if (filter == "A to Z" || filter == "Z to A")
                {
                    filter = "low to high";
                }

                if (filter == "low to high")
                {
                    filter = "high to low";
                }
                else
                    filter = "low to high";
            }

            HttpContext.Session.SetString("model", filter);

            return Redirect($"/product/index?page={page}&category={category}");
        }



        [HttpGet]
        public IActionResult Add()
        {
            var model = new ProductAddViewModel();
            model.Product = new Product();
            model.Categories = _categoryService.GetAll();
            return View(model);
        }
        [HttpPost]
        public IActionResult Add(ProductAddViewModel model)
        {
            _productService.Add(model.Product);
            return RedirectToAction("index");
        }
    }
}
