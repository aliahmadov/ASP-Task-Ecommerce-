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
        private IStateService _stateService;

        public ProductController(IProductService productService, ICategoryService categoryService, IStateService stateService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _stateService = stateService;
        }

        public IActionResult Index(int page = 1, int category = 0, bool azState = false, bool lhState = false)
        {
            var url = HttpContext.Request.QueryString.ToString();
            var products = _productService.GetAllByCategory(category);
            int pageSize = 10;

            if (url.Contains("azState"))
                products = _productService.GetProductsAZ(products, _stateService.CurrentState);
            else if(url.Contains("lhState"))
                products = _productService.GetProductsByFilterLoworHigh(products, _stateService.CurrentLHState);

            _stateService.SetState(url);
            var model = new ProductListViewModel()
            {
                Products = products.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                CurrentCategory = category,
                PageCount = (int)Math.Ceiling(products.Count / (double)pageSize),
                PageSize = pageSize,
                CurrentPage = page,
                AZState = _stateService.CurrentState,
                LHState = _stateService.CurrentLHState
            };

            return View(model);
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
