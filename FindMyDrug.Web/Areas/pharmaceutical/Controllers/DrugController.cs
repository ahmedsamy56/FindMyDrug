using ExcelDataReader;
using FindMyDrug.Entities.Models;
using FindMyDrug.Entities.Repositories;
using FindMyDrug.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FindMyDrug.Web.Areas.pharmaceutical.Controllers
{
    [Area("pharmaceutical")]
    [Authorize(Roles = $"{SD.pharmaceuticalRole}")]
    public class DrugController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DrugController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var pharmacy = _unitOfWork.Pharmacy.GetFirstorDefault(x=>x.Id ==id);
            if (pharmacy == null)
            {
                return NotFound();
            }
            ViewBag.pharmacyId = id;
            return View();
        }

        
        public IActionResult GetData(int id)
        {
            var Pharmacy = _unitOfWork.Pharmacy.GetFirstorDefault(x => x.Id == id);
            if(Pharmacy == null)
            {
                return NotFound();
            }
            else
            {

                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                string userId = claim.Value;

                if (Pharmacy.ApplicationUserId != userId)
                {
                    return Unauthorized();
                }
            }
            var drugs = _unitOfWork.Drug
                .GetAll(d => d.PharmacyId == id)
                .Select(d => new {
                    d.Id,
                    d.Name,
                    d.Dosage,
                    d.Price,
                    d.IsAvailable,
                    d.IsBlocked
                }).ToList();

            return Json(drugs);
        }




        public IActionResult AddDrug(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var pharmacy = _unitOfWork.Pharmacy.GetFirstorDefault(x => x.Id == id);
            if (pharmacy == null)
            {
                return NotFound();
            }
            ViewBag.pharmacyId = id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Drug drug)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Drug.Add(drug);
                int phId = (int)drug.PharmacyId;
                _unitOfWork.Complete();
                return RedirectToAction("Index", "Drug", new { area = "Pharmaceutical", id = phId });

            }
            return View("AddDrug", drug);
        }



        [HttpPost]
        public IActionResult ToggleAvailabilityAction(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drug = _unitOfWork.Drug.GetFirstorDefault(x => x.Id == id);
            if (drug == null)
            {
                return NotFound();
            }

            _unitOfWork.Drug.ToggleAvailability((int)id);

            _unitOfWork.Complete();

            return Ok(new { success = true});
        }


        [HttpGet]
        public IActionResult Update(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var drug = _unitOfWork.Drug.GetFirstorDefault(x=>x.Id == id);
            if (drug == null)
            {
                return NotFound();
            }
            return View(drug);
        }

        [HttpPost]
        public IActionResult Update(Drug drug)
        {
            if (ModelState.IsValid) 
            {
                _unitOfWork.Drug.Update(drug);
                int? phId = (int)drug.PharmacyId;
                _unitOfWork.Complete();
                
                return RedirectToAction("Index", "Drug", new { area = "Pharmaceutical", id = phId });
            }
            return View(drug);
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var drug = _unitOfWork.Drug.GetFirstorDefault(x=>x.Id==id);
            if (drug == null)
            {
                return NotFound();
            }
            _unitOfWork.Drug.Remove(drug);
            _unitOfWork.Complete();

            return Json(new { success = true});
        }




        [HttpPost]
        public IActionResult UploadExcel(int PharmacyId, IFormFile file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, file.FileName);

                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            bool isHeaderSkipped = false;

                            do
                            {
                                while (reader.Read())
                                {
                                    if (!isHeaderSkipped)
                                    {
                                        isHeaderSkipped = true;
                                        continue;
                                    }

                                    var s = new Drug
                                    {
                                        Name = reader.GetValue(1)?.ToString() ?? "Not Defined",
                                        Dosage = reader.GetValue(2)?.ToString() ?? "Not Defined",
                                        Price = reader.IsDBNull(3) ? 0 : (int.TryParse(reader.GetValue(3).ToString(), out var priceValue) ? priceValue : 0),
                                        IsAvailable = reader.GetValue(3) != null && Convert.ToInt32(reader.GetValue(4)) > 0,
                                        PharmacyId = PharmacyId
                                    };

                                    _unitOfWork.Drug.Add(s);
                                }
                            } while (reader.NextResult());

                            _unitOfWork.Complete();
                        }
                    }

                   
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    ViewBag.Message = "success";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = $"Error: {ex.Message}";
                }
            }
            else
            {
                ViewBag.Message = "empty";
            }

            return RedirectToAction("Index", "Drug", new { area = "Pharmaceutical", id = PharmacyId });
        }



    }

}
