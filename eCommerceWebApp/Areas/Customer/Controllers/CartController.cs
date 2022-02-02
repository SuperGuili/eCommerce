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
                ListCart = _unitOfWork.ShoppingCart.GetAll(u=>u.ApplicationUserId == userID, includeProperties:"Product")                
            };

            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price10, cart.Product.Price20);

                ShoppingCartVM.CartTotal += (cart.Count * cart.Price);
            }

            return View(ShoppingCartVM);
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
