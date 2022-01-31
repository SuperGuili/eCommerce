using eCommerce.DataAccess;
using eCommerce.DataAccess.Repository.IRepository;
using eCommerce.Models;
using eCommerce.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eCommerceWebApp.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork dbContext, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DetailsCompany(int id)
        {
            Company company = _unitOfWork.Company.GetFirstOrDefault(x => x.Id == id);

            return View(company);
        }

        [HttpGet]
        public IActionResult UpsertCompany(int? id)
        {
            Company company = new();

            if (id == null || id == 0)
            {
                //Create company
                return View(company);
            }
            else
            {
                //Update company
                company = _unitOfWork.Company.GetFirstOrDefault(p => p.Id == id);

                return View(company);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpsertCompany(Company model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id==0)
                {
                    _unitOfWork.Company.Add(model);
                    TempData["success"] = "Company Added Succesfully";
                }
                else
                {
                    _unitOfWork.Company.Update(model);
                    TempData["success"] = "Company Updated Succesfully";
                }
                
                _unitOfWork.Save();                

                return RedirectToAction("Index");
            }

            return View(model);
        }

        
        #region API CALLS

        [HttpGet]
        public IActionResult GetAllCompanies()
        {
            var companyList = _unitOfWork.Company.GetAll();

            return Json(new {data= companyList });
        }

        [HttpDelete]
        public IActionResult DeleteCompany(int? id)
        {

            var companyFromDb = _unitOfWork.Company.GetFirstOrDefault(c => c.Id == id);

            if (companyFromDb == null)
            {
                return Json(new {success=false, message="Error while deleting"});
            }

            _unitOfWork.Company.Remove(companyFromDb);

            _unitOfWork.Save();

            //TempData["success"] = "Category Deleted Succesfully";

            return Json(new {success=true, message= "Company deleted successfully" });

        }

        #endregion

    }
}
