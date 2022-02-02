using eCommerce.DataAccess.Repository.IRepository;
using eCommerce.Models;
using eCommerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace eCommerceWebApp.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties:"Category,Tag");    

            return View(productList);
        }

        [HttpGet]
        public IActionResult DetailsProduct(int productId)
        {
            ShoppingCart shoppingCart = new()
            {
                Product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == productId, includeProperties: "Category,Tag"),
                ProductId = productId,
                Count = 1
            };

            if(shoppingCart.Product.TagId2 != 0)
            {
                ViewBag.Tag2 = _unitOfWork.Tag.GetFirstOrDefault(t => t.Id == shoppingCart.Product.TagId2).TagName;
            }
            if (shoppingCart.Product.TagId3 != 0)
            {
                ViewBag.Tag3 = _unitOfWork.Tag.GetFirstOrDefault(t => t.Id == shoppingCart.Product.TagId3).TagName;
            }

            return View(shoppingCart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult DetailsProduct(ShoppingCart model)
        {
            //Workaround to avoid changing the product table error
            model.Product = null;

            // Procedure to get the user ID
            var clainsIdentity = (ClaimsIdentity)User.Identity;
            var claim = clainsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //Add User ID to the shopping cart
            model.ApplicationUserId = claim.Value;

            //Check for an existing shoppingCart 
            ShoppingCart shoppingCartDB = _unitOfWork.ShoppingCart.GetFirstOrDefault(
                u=> u.ApplicationUserId == claim.Value && u.ProductId == model.ProductId
                );

            if (shoppingCartDB == null)
            {
                _unitOfWork.ShoppingCart.Add(model);
            }
            else
            {
                _unitOfWork.ShoppingCart.IncrementCount(shoppingCartDB, model.Count);
            }

            _unitOfWork.Save();

            return RedirectToAction("Index");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}