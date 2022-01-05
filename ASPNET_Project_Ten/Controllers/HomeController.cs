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
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.DataProtection;
using ASPNET_Project_Eleven.Security;

namespace ASPNET_Project_Eleven.Controllers
{

    [Authorize(Policy = "AdminLevelPolicy")]
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly IDatabaseRepository _databaseRepository;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly IConfiguration configuration;
        private readonly IDataProtector protector;

        public HomeController(  IDatabaseRepository employeeRepository,
                                IWebHostEnvironment hostingEnvironment,
                                IConfiguration configuration,
                                IDataProtectionProvider dataProtectionProvider,
                                DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _databaseRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
            this.configuration = configuration;
            protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.EmployeeIdRouteValue);
        }

        [Route("~/")]
        [Route("~/Home")]
        [Route("")]
        [AllowAnonymous]
        [ActionName("Index")]
        public IActionResult Index()
        {
            string projectRootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            projectRootPath = Path.Combine(projectRootPath, "../../../");

            string anotherPath = Environment.CurrentDirectory.ToString();
            anotherPath = Path.Combine(anotherPath, "MyDatabase.db");
            anotherPath = "Data Source=file:///" + anotherPath;
            // ViewBag.RootPath = configuration["Production:SqliteConnectionStringSQLite"];
            ViewBag.RootPath = anotherPath;
            return View();
        }

        [AllowAnonymous]
        public IActionResult About()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        public IActionResult ListEmployees()
        {
            var model = _databaseRepository.GetAllEmployees().Select(emp => {
                emp.EncryptedId = protector.Protect(emp.Id.ToString());
                return emp;
            });
            return View(model);
        }

        [Route("{id}")]
        [AllowAnonymous]
        [ActionName(nameof(DetailsEmployee))]
        // public ViewResult DetailsEmployee(int id)
        public ViewResult DetailsEmployee(string id)
        {
            // throw new Exception("Error in Details View");

            int employeeId = Convert.ToInt32(protector.Unprotect(id));

            Employee employee = _databaseRepository.GetEmployee(employeeId);

            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", employeeId);
            }

            employee.EncryptedId = id;

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = employee,
                // _employeeRepository.GetEmployee(id??1),
                PageTitle = "Employee Details"
            };
            return View(homeDetailsViewModel);
        }

        [HttpGet]
        [ActionName(nameof(CreateEmployee))]
        public ViewResult CreateEmployee()
        {
            return View();
        }

        [HttpPost]
        [ActionName(nameof(CreateEmployee))]
        public IActionResult CreateEmployee(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);
                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFileName
                };
                _databaseRepository.AddEmployee(newEmployee);
                //  _employeeRepository.Add(employee);
                return RedirectToAction("ListEmployees", new { id = newEmployee.Id });
            }

            return View();
        }

        [HttpGet]
        [ActionName("EditEmployee")]
        public ViewResult EditEmployee(string id)
        {
            int decryptedId = Convert.ToInt32(protector.Unprotect(id));

            Employee employee = _databaseRepository.GetEmployee(decryptedId);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel()
            {
                EncryptedId = id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath = employee.PhotoPath
            };
            return View(employeeEditViewModel);
        }

        [HttpPost]
        [ActionName("EditEmployee")]
        public IActionResult EditEmployee(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                int employeeId = Convert.ToInt32(protector.Unprotect(model.EncryptedId));
                Employee employee = _databaseRepository.GetEmployee(employeeId);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;

                if (model.Photo != null)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath,
                            "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    employee.PhotoPath = ProcessUploadedFile(model);
                }

                Employee updatedEmployee = _databaseRepository.UpdateEmployee(employee);

                return RedirectToAction("ListEmployees");
            }

            return View(model);
        }

        [Route("{id}")]
        [HttpPost]
        [ActionName("DeleteEmployee")]
        public IActionResult DeleteEmployee(string id)
        {
            int employeeId = Convert.ToInt32(protector.Unprotect(id));
            _databaseRepository.DeleteEmployee(employeeId);
            return RedirectToAction("ListEmployees");
        }

        private string ProcessUploadedFile(EmployeeCreateViewModel model)
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

