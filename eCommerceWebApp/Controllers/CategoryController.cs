using eCommerce.DataAccess;
using eCommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceWebApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categoryList = _dbContext.Categories.OrderBy(c => c.DisplayOrder);

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

                _dbContext.Categories.Add(category);

                _dbContext.SaveChanges();

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

            Category category = _dbContext.Categories.Find(Id);
            //Category category = _dbContext.Categories.FirstOrDefault(c => c.Id==Id);
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
                _dbContext.Categories.Update(model);

                _dbContext.SaveChanges();

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

            Category category = _dbContext.Categories.Find(Id);
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
            _dbContext.Categories.Remove(model);

            _dbContext.SaveChanges();

            TempData["success"] = "Category Deleted Succesfully";

            return RedirectToAction("Index");

        }
    }
}
