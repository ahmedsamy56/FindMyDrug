using FindMyDrug.Entities.Repositories;
using FindMyDrug.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FindMyDrug.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.AdminRole}")]
    public class DrugController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public DrugController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var Drugs = _unitOfWork.Drug.GetAll(Includeword: "Pharmacy");
            return View(Drugs); 
        }


        [HttpPost]
        public IActionResult ToggleBlockStatusAction(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false });
            }

            var drug = _unitOfWork.Drug.GetFirstorDefault(x => x.Id == id);
            if (drug == null)
            {
                return Json(new { success = false });
            }

            _unitOfWork.Drug.ToggleBlockStatus((int)id);
            _unitOfWork.Complete();

            return Json(new { success = true });
        }
    }
}
