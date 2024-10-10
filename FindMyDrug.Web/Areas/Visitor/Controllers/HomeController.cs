using FindMyDrug.Entities.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FindMyDrug.Web.Areas.Visitor.Controllers
{
    [Area("Visitor")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var drugs = _unitOfWork.Drug.GetAll(x => x.IsAvailable == true && x.IsBlocked == false  , Includeword: "Pharmacy");
            return View(drugs);
        }
    }
}
