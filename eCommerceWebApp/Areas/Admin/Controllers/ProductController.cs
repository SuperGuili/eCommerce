using eCommerce.DataAccess;
using eCommerce.DataAccess.Repository.IRepository;
using eCommerce.Models;
using eCommerce.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eCommerceWebApp.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork dbContext, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = dbContext;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll();

            return View(productList);
        }

        [HttpGet]
        public IActionResult UpsertProduct(int? Id)
        {
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem
                {
                    Text = c.CategoryName,
                    Value = c.Id.ToString()
                }),
                TagList = _unitOfWork.Tag.GetAll().Select(c => new SelectListItem
                {
                    Text = c.TagName,
                    Value = c.Id.ToString()
                }),
            };

            if (Id == null || Id == 0)
            {
                //Create product
                return View(productVM);
            }
            else
            {
                //Update Product
            }



            return View(productVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpsertProduct(ProductVM model, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString(); //Generate GUID for the file name.
                    var uploadsPath = Path.Combine(wwwRootPath, @"images\products"); //Find the path to save file.
                    var extension = Path.GetExtension(file.FileName); //Get the file extension to keep it the same

                    //Create a file stream object to save the file
                    using (var fileStreams = new FileStream(Path.Combine(uploadsPath, fileName + extension), FileMode.Create))
                    {
                        //Copy the file
                        file.CopyTo(fileStreams);
                    }
                    //Save the file path and name on the database
                    model.Product.ImageUrl = @"\images\products\" + fileName + extension;

                }

                _unitOfWork.Product.Add(model.Product);

                _unitOfWork.Save();

                TempData["success"] = "Product Created Succesfully";

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
