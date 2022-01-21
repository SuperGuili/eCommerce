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
            return View();
        }

        [HttpGet]
        public IActionResult UpsertProduct(int? id)
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

            if (id == null || id == 0)
            {
                //Create product
                return View(productVM);
            }
            else
            {
                //Update Product
                productVM.Product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == id);

                return View(productVM);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpsertProduct(ProductVM model, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                //Get the wwwroot path
                string wwwRootPath = _hostEnvironment.WebRootPath;

                //Check if the product already have a file
                if (model.Product.ImageUrl != null && file != null)
                {
                    //Get the file path for the file on database
                    var oldImagePath = Path.Combine(wwwRootPath, model.Product.ImageUrl.TrimStart('\\'));
                    //Check if the file exists
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        //Delete the file before updating
                        System.IO.File.Delete(oldImagePath);
                    }
                }

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
                if (model.Product.Id==0)
                {
                    _unitOfWork.Product.Add(model.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(model.Product);
                }
                
                _unitOfWork.Save();

                TempData["success"] = "Product Updated Succesfully";

                return RedirectToAction("Index");
            }

            return View(model);
        }

        //[HttpGet]
        //public IActionResult DeleteCategory(int? Id)
        //{
        //    if (Id == null || Id == 0)
        //    {
        //        return NotFound();
        //    }

        //    Category category = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == Id);
        //    //Category category = _dbContext.Categories.FirstOrDefault(c => c.Id==Id);
        //    //Category category = _dbContext.Categories.SingleOrDefault(c => c.Id==Id);

        //    if (category == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(category);

        //}

        
        #region API CALLS

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var productList = _unitOfWork.Product.GetAll(includeProperties:"Category,Tag");

            return Json(new {data=productList});
        }

        [HttpDelete]
        public IActionResult DeleteProduct(int? id)
        {

            var productFromDb = _unitOfWork.Product.GetFirstOrDefault(c => c.Id == id);

            if (productFromDb == null)
            {
                return Json(new {success=false, message="Error while deleting"});
            }

            //Get the file path for the file on database
            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, productFromDb.ImageUrl.TrimStart('\\'));
            //Check if the file exists
            if (System.IO.File.Exists(oldImagePath))
            {
                //Delete the file before updating
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(productFromDb);

            _unitOfWork.Save();

            //TempData["success"] = "Category Deleted Succesfully";

            return Json(new {success=true, message="Product deleted successfully"});

        }

        #endregion

    }
}
