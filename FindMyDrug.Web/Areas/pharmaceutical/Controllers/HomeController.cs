using FindMyDrug.Entities.Repositories;
using FindMyDrug.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FindMyDrug.Entities.Models;

namespace FindMyDrug.Web.Areas.pharmaceutical.Controllers
{
    [Area("pharmaceutical")]
    [Authorize(Roles = $"{SD.pharmaceuticalRole}")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            string userId = null;

            if (User?.Identity is ClaimsIdentity claimsIdentity)
            {
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                if (claim != null)
                {
                    userId = claim.Value;
                }
            }

            var pharmacies = _unitOfWork.Pharmacy.GetAll().Where(x => x.ApplicationUserId == userId);

            return View(pharmacies);
        }
    }
}
