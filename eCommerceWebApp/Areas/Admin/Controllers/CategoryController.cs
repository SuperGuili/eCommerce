using eCommerce.DataAccess;
using eCommerce.DataAccess.Repository.IRepository;
using eCommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceWebApp.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork dbContext)
        {
            _unitOfWork = dbContext;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categoryList = _unitOfWork.Category.GetAll();

            return View(categoryList);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCategory(Category model)
        {
            if (ModelState.IsValid)
            {
                Category category = new Category
                {
                    CategoryName = model.CategoryName,
                    DisplayOrder = model.DisplayOrder
                };

                _unitOfWork.Category.Add(category);

                _unitOfWork.Save();

                TempData["success"] = "Category Created Succesfully";

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult EditCategory(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            //Category category = _dbContext.Categories.Find(Id);
            Category category = _unitOfWork.Category.GetFirstOrDefault(c => c.Id==Id);
            //Category category = _dbContext.Categories.SingleOrDefault(c => c.Id==Id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCategory(Category model)
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
