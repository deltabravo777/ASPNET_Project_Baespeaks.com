using ASPNET_Project_Eleven.Models;
using ASPNET_Project_Eleven.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.DataProtection;
using ASPNET_Project_Eleven.Security;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASPNET_Project_Eleven.Controllers
{
    [Authorize(Policy = "ManagerLevelPolicy")]
    [Route("[controller]/[action]")]
    public class PortfolioController : Controller
    {

        private readonly IDatabaseRepository _databaseRepository;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly IDataProtector protector;

        public PortfolioController(IDatabaseRepository employeeRepository,
                                IWebHostEnvironment hostingEnvironment,
                                IDataProtectionProvider dataProtectionProvider,
                                DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _databaseRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
            protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.BlueprintIdRouteValue);
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            // throw new Exception("huh");
            return View();
        }

        [AllowAnonymous]
        [ActionName("ListBlueprints")]
        public IActionResult ListBlueprints()
        {
            IEnumerable<Blueprint> model = _databaseRepository.GetAllBlueprints();
            // throw new Exception("huh");
            return View(model);
        }

        [AllowAnonymous]
        [ActionName("ListEditBlueprints")]
        public IActionResult ListEditBlueprints()
        {
            IEnumerable<Blueprint> model = _databaseRepository.GetAllBlueprints();
            return View(model);
        }

        [Route("{id}")]
        [AllowAnonymous]
        [ActionName("DetailsBlueprint")]
        public ViewResult DetailsBlueprint(Guid id)
        {
            // int blueprintId = Convert.ToInt32(protector.Unprotect(id));
            Blueprint blueprint = _databaseRepository.GetBlueprint(id);
            if (blueprint == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id);
            }

            BlueprintDetailsViewModel blueprintDetailsViewModel = new BlueprintDetailsViewModel()
            {
                Blueprint = blueprint
            };
            return View(blueprintDetailsViewModel);
        }

        [HttpGet]
        [ActionName(nameof(CreateBlueprint))]
        public ViewResult CreateBlueprint()
        {
            var blueprints = _databaseRepository.GetAllBlueprints();

            double maxNumber;
            if (blueprints.Any())
            {
                maxNumber = (from Blueprint b in blueprints
                             select b.Rank).Max();
            }
            else
            {
                maxNumber = 0;
            }
            ViewBag.MaxRank = maxNumber;
            return View();
        }

        [HttpPost]
        [ActionName(nameof(CreateBlueprint))]
        public IActionResult CreateBlueprint(BlueprintCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);
                Blueprint blueprint = new Blueprint
                {
                    Id = Guid.NewGuid(),
                    Title = model.Title,
                    BlueprintBody = model.BlueprintBody,
                    PhotoPath = uniqueFileName,
                    Rank = model.Rank,
                    Category = model.Category
                };
                _databaseRepository.AddBlueprint(blueprint);
                return RedirectToAction("DetailsBlueprint", new { id = blueprint.Id });
            }

            return View();
        }

        [HttpGet]
        [ActionName("EditBlueprint")]
        public ViewResult EditBlueprint(Guid id)
        {
            // int blueprintId = Convert.ToInt32(protector.Unprotect(id));
            Blueprint blueprint = _databaseRepository.GetBlueprint(id);
            BluePrintEditViewModel blueprintEditViewModel = new BluePrintEditViewModel()
            {
                Id = blueprint.Id,
                Title = blueprint.Title,
                BlueprintBody = blueprint.BlueprintBody,
                ExistingPhotoPath = blueprint.PhotoPath,
                Rank = blueprint.Rank,
                Category = blueprint.Category
            };
            return View(blueprintEditViewModel);
        }

        [HttpPost]
        [ActionName("EditBlueprint")]
        public IActionResult EditBlueprint(BluePrintEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Blueprint blueprint = _databaseRepository.GetBlueprint(model.Id);
                blueprint.Title = model.Title;
                blueprint.BlueprintBody = model.BlueprintBody;
                blueprint.Rank = model.Rank;
                blueprint.Category = model.Category;

                if (model.Photo != null)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath,
                            "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    blueprint.PhotoPath = ProcessUploadedFile(model);
                }

                Blueprint updatedBlueprint = _databaseRepository.UpdateBlueprint(blueprint);

                return RedirectToAction("ListEditBlueprints");
            }

            return View(model);
        }

        [HttpPost]
        [ActionName("DeleteBlueprint")]
        [Authorize(Policy = "AdminLevelPolicy")]
        public IActionResult DeleteBlueprint(Guid id)
        {
            if (id.Equals(null))
            {
                throw new Exception();
            }
            _databaseRepository.DeleteBlueprint(id);
            return RedirectToAction("ListBlueprints");
        }

        private string ProcessUploadedFile(BlueprintCreateViewModel model)
        {
            string uniqueFileName = null;

            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}

