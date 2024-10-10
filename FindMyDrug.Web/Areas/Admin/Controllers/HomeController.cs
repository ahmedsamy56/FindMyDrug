using FindMyDrug.Entities.Repositories;
using FindMyDrug.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FindMyDrug.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.AdminRole}")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
           ViewBag.UserCount = _unitOfWork.ApplicationUser.GetAll().Count();
           ViewBag.PharmacyCount = _unitOfWork.Pharmacy.GetAll().Count();
           ViewBag.DrugCount = _unitOfWork.Drug.GetAll().Count();
            var LastDrugs = _unitOfWork.Drug
                             .GetAll(x => x.IsAvailable == true && x.IsBlocked == false , Includeword: "Pharmacy")
                             .OrderByDescending(d => d.Id) 
                             .Take(4)
                             .ToList();

            return View(LastDrugs);
        }
    }
}
