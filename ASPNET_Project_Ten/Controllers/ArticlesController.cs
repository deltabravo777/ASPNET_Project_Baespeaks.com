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

namespace ASPNET_Project_Eleven.Controllers
{
    [Authorize(Policy = "ManagerLevelPolicy")]
    [Route("[controller]/[action]")]
    public class ArticlesController : Controller
    {
        private readonly IDatabaseRepository _databaseRepository;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly IDataProtector protector;

        public ArticlesController(  IDatabaseRepository employeeRepository,
                                    IWebHostEnvironment hostingEnvironment,
                                    IDataProtectionProvider dataProtectionProvider,
                                    DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _databaseRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
            protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.ArticleIdRouteValue);
        }

        // GET: /<controller>/
        [AllowAnonymous]
        [ActionName("ListArticles")]
        public IActionResult ListArticles()
        {
            IEnumerable<Article> model = _databaseRepository.GetAllArticles().Select(art => {
                art.EncryptedId = protector.Protect(art.Id.ToString());
                art.Id = 0;
                return art;
            });
            return View(model);
        }

        [AllowAnonymous]
        [ActionName("ListEditArticles")]
        public IActionResult ListEditArticles()
        {
            IEnumerable<Article> model = _databaseRepository.GetAllArticles().Select(art => {
                art.EncryptedId = protector.Protect(art.Id.ToString());
                art.Id = 0;
                return art;
            });
            return View(model);
        }

        [Route("{id}")]
        [AllowAnonymous]
        [ActionName("DetailsArticle")]
        public ViewResult DetailsArticle(string id)
        {
            
            int decryptedId = 0;
            Article article = null;

            decryptedId = Convert.ToInt32(protector.Unprotect(id));
            article = _databaseRepository.GetArticle(decryptedId);
            #region bug fixed
            /*
            throw new Exception("huh");
            if (id.Length > 10)
            {
                decryptedId = Convert.ToInt32(protector.Unprotect(id));
                article = _databaseRepository.GetArticle(decryptedId);
            }
            else
            {
                article = _databaseRepository.GetArticle(Int32.Parse(id));
            }
            */
            // article.ArticleBody = article.ArticleBody.Replace(System.Environment.NewLine, "<br />");
            #endregion

            if (article == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", decryptedId);
            }

            // we hide the Id and set it to 0 in a new instance of an article
            article.Id = 0;
            ArticleDetailsViewModel articleDetailsViewModel = new ArticleDetailsViewModel()
            {
                Article = article
            };
            // throw new Exception("huh 2");
            return View(articleDetailsViewModel);
        }

        [HttpGet]
        [ActionName(nameof(CreateArticle))]
        public ViewResult CreateArticle()
        {
            var articles = _databaseRepository.GetAllArticles();

            double maxNumber;
            if (articles.Any())
            {
                maxNumber = (from Article a in articles
                             select a.Rank).Max();
            }
            else
            {
                maxNumber = 0;
            }
            ViewBag.MaxRank = maxNumber;
            return View();
        }

        [HttpPost]
        [ActionName(nameof(CreateArticle))]
        public IActionResult CreateArticle(ArticleCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);
                int convertedMonth = (int)model.Month;
                // DateTime dateCreated = new DateTime(model.Year, convertedMonth, model.Day);
                Article article = new Article
                {
                    Title = model.TitleName,
                    ArticleBody = model.ArticleBody,
                    PhotoPath = uniqueFileName,
                    Year = model.Year,
                    Month = model.Month,
                    Day = model.Day,
                    Rank = model.Rank,
                    Category = model.Category
                };
                _databaseRepository.AddArticle(article);
                return RedirectToAction("DetailsArticle", new { id = protector.Protect(article.Id.ToString()) });
            }

            return View();
        }

        [HttpGet]
        //[HttpGet]
        public IActionResult EditArticle(string id)
        {
            int decryptedId = Convert.ToInt32(protector.Unprotect(id));
            Article article = _databaseRepository.GetArticle(decryptedId);
            ArticleEditViewModel articleEditViewModel = new ArticleEditViewModel()
            {
                EncryptedId = id,
                Year = article.Year,
                Month = article.Month,
                Day = article.Day,
                TitleName = article.Title,
                ArticleBody = article.ArticleBody,
                ExistingPhotoPath = article.PhotoPath,
                Rank = article.Rank,
                Category = article.Category
            };

            // return Json(articleEditViewModel);

            return View(articleEditViewModel);
        }

        [HttpPost]
        [ActionName("EditArticle")]
        //[Route("~/Articles/EditArticle/{id}")]
        public IActionResult EditArticle(ArticleEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Article article = _databaseRepository.GetArticle(Convert.ToInt32(protector.Unprotect(model.EncryptedId)));
                article.Title = model.TitleName;
                article.ArticleBody = model.ArticleBody;
                article.Category = model.Category;
                article.Year = model.Year;
                article.Month = model.Month;
                article.Day = model.Day;
                article.Rank = model.Rank;

                if (model.Photo != null)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath,
                            "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    article.PhotoPath = ProcessUploadedFile(model);
                }

                Article updatedEmployee = _databaseRepository.UpdateArticle(article);

                return RedirectToAction("ListEditArticles");
            }

            return View(model);
        }

        [Route("{id}")]
        [HttpPost]
        [ActionName("DeleteArticle")]
        [Authorize(Policy = "AdminLevelPolicy")]
        public IActionResult DeleteArticle(string id)
        {
            int decryptedId = Convert.ToInt32(protector.Unprotect(id));
            _databaseRepository.DeleteArticle(decryptedId);
            return RedirectToAction("ListArticles");
        }

        private string ProcessUploadedFile(ArticleCreateViewModel model)
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

