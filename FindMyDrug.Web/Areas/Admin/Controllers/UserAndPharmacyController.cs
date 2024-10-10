using FindMyDrug.Entities.Repositories;
using FindMyDrug.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FindMyDrug.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.AdminRole}")]
    public class UserAndPharmacyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserAndPharmacyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userid = claim.Value;
            var users = _unitOfWork.ApplicationUser.GetAll(Includeword: "pharmacies").Where(x=>x.Id != userid).ToList();
            return View(users);
        }
    }
}
