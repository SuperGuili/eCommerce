using eCommerce.DataAccess.Repository.IRepository;
using eCommerce.Models;
using eCommerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eCommerceWebApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }


        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; 
        }

        public IActionResult Index()
        {
            // Procedure to get the user ID
            var clainsIdentity = (ClaimsIdentity)User.Identity;
            var claim = clainsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userID = claim.Value;

            ShoppingCartVM = new()
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userID, includeProperties: "Product"),
                OrderHeader = new()
            };

            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price10, cart.Product.Price20);

                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Count * cart.Price);
            }

            return View(ShoppingCartVM);
        }

        [HttpGet]
        public IActionResult Summary()
        {
            // Procedure to get the user ID
            var clainsIdentity = (ClaimsIdentity)User.Identity;
            var claim = clainsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userID = claim.Value;

            ShoppingCartVM = new()
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userID, includeProperties: "Product"),
                OrderHeader = new()
            };

            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == userID);

            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.Address = ShoppingCartVM.OrderHeader.ApplicationUser.Address;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.PostCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostCode;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;


            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price10, cart.Product.Price20);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Count * cart.Price);
            }

            return View(ShoppingCartVM);

            return View();
        }


        public IActionResult Plus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.Id == cartId, includeProperties: "Product");

            if (cartFromDb.Product.StockQuantity > cartFromDb.Count)
            {
                _unitOfWork.ShoppingCart.IncrementCount(cartFromDb, 1);
                _unitOfWork.Save();
            }
            else
            {
                TempData["error"] = "Maximum quantity available in stock";
            }            

            return RedirectToAction(nameof(Index));

        }

        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.Id == cartId);

            if (cartFromDb.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(cartFromDb);
            }
            else
            {
                _unitOfWork.ShoppingCart.DecrementCount(cartFromDb, 1);
            }           

            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));

        }

        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.Id == cartId);

            _unitOfWork.ShoppingCart.Remove(cartFromDb);

            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));

        }


        public double GetPriceBasedOnQuantity(double quantity, double price, double price10, double price20)
        {
            if (quantity <=10)
            {
                return price;
            }
            else
            {
                if (quantity <= 20)
                {
                    return price10;
                }
                return price20;
            }
        }
    }
}
