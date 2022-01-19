using eCommerce.DataAccess;
using eCommerce.DataAccess.Repository.IRepository;
using eCommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eCommerceWebApp.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork dbContext)
        {
            _unitOfWork = dbContext;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll();

            return View(productList);
        }

        [HttpGet]
        public IActionResult UpsertProduct(int? Id)
        {
            Product product = new();
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(
                c => new SelectListItem
                {
                    Text = c.CategoryName,
                    Value = c.Id.ToString()
                });

            IEnumerable<SelectListItem> TagList = _unitOfWork.Tag.GetAll().Select(
                t => new SelectListItem
                {
                    Text = t.TagName,
                    Value = t.Id.ToString()
                });

            if (Id == null || Id == 0)
            {
                //Create product
                ViewBag.CategoryList = CategoryList;
                return View(product);
            }
            else
            {
                //Update Product
            }



            return View(product);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpsertProduct(Category model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(model);

                _unitOfWork.Save();

                TempData["success"] = "Category Updated Succesfully";

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult DeleteCategory(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            Category category = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == Id);
            //Category category = _dbContext.Categories.FirstOrDefault(c => c.Id==Id);
            //Category category = _dbContext.Categories.SingleOrDefault(c => c.Id==Id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);

        }

        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(Category model)
        {
            if (model == null)
            {
                return NotFound();
            }

            var obj = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == model.Id);

            _unitOfWork.Category.Remove(obj);

            _unitOfWork.Save();

            TempData["success"] = "Category Deleted Succesfully";

            return RedirectToAction("Index");

        }
    }
}
