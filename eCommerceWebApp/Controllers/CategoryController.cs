using eCommerceWebApp.Data;
using eCommerceWebApp.Models;
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
            IEnumerable<Category> categoryList = _dbContext.Categories;

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

                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}
