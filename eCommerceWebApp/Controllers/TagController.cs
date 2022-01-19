using eCommerce.DataAccess.Repository.IRepository;
using eCommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceWebApp.Controllers
{
    public class TagController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TagController(IUnitOfWork db)
        {
            _unitOfWork = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Tag> TagList = _unitOfWork.Tag.GetAll();

            return View(TagList);
        }

        [HttpGet]
        public IActionResult CreateTag()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTag(Tag model)
        {
            if (ModelState.IsValid)
            {
                Tag tag = new Tag
                {
                    TagName = model.TagName                    
                };

                _unitOfWork.Tag.Add(tag);

                _unitOfWork.Save();

                TempData["success"] = "Tag Created Succesfully";

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult EditTag(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            //Category category = _dbContext.Categories.Find(Id);
            Tag tag = _unitOfWork.Tag.GetFirstOrDefault(t => t.Id == Id);
            //Category category = _dbContext.Categories.SingleOrDefault(c => c.Id==Id);

            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTag(Tag model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Tag.Update(model);

                _unitOfWork.Save();

                TempData["success"] = "Tag Updated Succesfully";

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult DeleteTag(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            Tag tag = _unitOfWork.Tag.GetFirstOrDefault(t => t.Id == Id);
            //Category category = _dbContext.Categories.FirstOrDefault(c => c.Id==Id);
            //Category category = _dbContext.Categories.SingleOrDefault(c => c.Id==Id);

            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);

        }

        [HttpPost, ActionName("DeleteTag")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteTag(Tag model)
        {
            if (model == null)
            {
                return NotFound();
            }

            Tag tag = _unitOfWork.Tag.GetFirstOrDefault(t => t.Id == model.Id);

            _unitOfWork.Tag.Remove(tag);

            _unitOfWork.Save();

            TempData["success"] = "Tag Deleted Succesfully";

            return RedirectToAction("Index");

        }
    }
}
