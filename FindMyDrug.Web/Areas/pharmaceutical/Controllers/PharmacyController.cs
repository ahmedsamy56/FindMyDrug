using FindMyDrug.Entities.Models;
using FindMyDrug.Entities.Repositories;
using FindMyDrug.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FindMyDrug.Web.Areas.pharmaceutical.Controllers
{
    [Area("pharmaceutical")]
    [Authorize(Roles = $"{SD.pharmaceuticalRole}")]
    public class PharmacyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PharmacyController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(Pharmacy pharmacy, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string RootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var Upload = Path.Combine(RootPath, @"Images\Pharmacies");
                    var ext = Path.GetExtension(file.FileName);

                    using (var filestream = new FileStream(Path.Combine(Upload, filename + ext), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    pharmacy.Img = @"Images\Pharmacies\" + filename + ext;
                }

                if (pharmacy.IsOpen24Hrs)
                {
                    pharmacy.OpenHour = null;
                    pharmacy.CloseHour = null;
                }

               

                //Add ApplicationUserId

                string userId = null;

                if (User?.Identity is ClaimsIdentity claimsIdentity)
                {
                    var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                    if (claim != null)
                    {
                        userId = claim.Value;
                    }
                }

                pharmacy.ApplicationUserId = userId;


                _unitOfWork.Pharmacy.Add(pharmacy);
                _unitOfWork.Complete();
                return RedirectToAction("Index" , "Home");
            }
            return View("Add", pharmacy);
        }


        [AllowAnonymous]
        public IActionResult View(int id)
        {
            var pharmacy = _unitOfWork.Pharmacy.GetFirstorDefault(x=>x.Id == id);

            if (pharmacy == null)
            {
                return NotFound();
            }

            return View(pharmacy);
        }


        public IActionResult Delete(int id)
        {
            var pharmacy = _unitOfWork.Pharmacy.GetFirstorDefault(x => x.Id == id);
            if (pharmacy == null)
            {
                return NotFound();
            }

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = claim.Value;

            if (pharmacy.ApplicationUserId != userId)
            {
                return Unauthorized();
            }

            string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, pharmacy.Img);

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            _unitOfWork.Pharmacy.Remove(pharmacy);
            _unitOfWork.Complete();

            return RedirectToAction("Index", "Home");
        }



        [HttpGet]
        public IActionResult Update(int id)
        {
            var pharmacy = _unitOfWork.Pharmacy.GetFirstorDefault(x => x.Id == id);
            if (pharmacy == null)
            {
                return NotFound();
            }

            return View(pharmacy); 
        }

        [HttpPost]
        public IActionResult Update(Pharmacy pharmacy, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                return View(pharmacy);
            }
            string RootPath = _webHostEnvironment.WebRootPath;
            // Handle file upload
            if (file != null)
            {
                string filename = Guid.NewGuid().ToString();
                var UploadPath = Path.Combine(RootPath, @"Images\Pharmacies");
                var ext = Path.GetExtension(file.FileName);

                if (!string.IsNullOrEmpty(pharmacy.Img))
                {
                    var oldImgPath = Path.Combine(RootPath, pharmacy.Img.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImgPath))
                    {
                        System.IO.File.Delete(oldImgPath);
                    }
                }


                using (var filestream = new FileStream(Path.Combine(UploadPath, filename + ext), FileMode.Create))
                {
                    file.CopyTo(filestream);
                }


                pharmacy.Img = @"Images\Pharmacies\" + filename + ext;
            }
 

            if (pharmacy.IsOpen24Hrs)
            {
                pharmacy.OpenHour = null;
                pharmacy.CloseHour = null;
            }


            _unitOfWork.Pharmacy.Update(pharmacy);
            _unitOfWork.Complete();
            return RedirectToAction("Index", "Home");
        }

    }
}
