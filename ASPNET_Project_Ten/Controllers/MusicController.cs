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
    public class MusicController : Controller
    {
        private readonly IDatabaseRepository _databaseRepository;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly IDataProtector protector;

        public MusicController( IDatabaseRepository employeeRepository,
                                IWebHostEnvironment hostingEnvironment,
                                IDataProtectionProvider dataProtectionProvider,
                                DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _databaseRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
            protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.PlaylistIdRouteValue);
        }

        // GET: /<controller>/
        [AllowAnonymous]
        [ActionName("ListPlaylists")]
        public IActionResult ListPlaylists()
        {
            IEnumerable<Playlist> model = _databaseRepository.GetAllPlaylists().Select(playlist => {
                playlist.EncryptedId = protector.Protect(playlist.Id.ToString());
                playlist.Id = 0;
                return playlist;
            });
            return View(model);
        }

        [AllowAnonymous]
        [ActionName("ListEditPlaylists")]
        public IActionResult ListEditPlaylists()
        {
            IEnumerable<Playlist> model = _databaseRepository.GetAllPlaylists().Select(playlist => {
                playlist.EncryptedId = protector.Protect(playlist.Id.ToString());
                playlist.Id = 0;
                return playlist;
            });
            return View(model);
        }

        [Route("{id}")]
        [AllowAnonymous]
        [ActionName("DetailsPlaylist")]
        public ViewResult DetailsPlaylist(string id)
        {
            int playlistId = Convert.ToInt32(protector.Unprotect(id));
            Playlist playlist = _databaseRepository.GetPlaylist(playlistId);

            // we hide the Id and set it to 0 in a new instance of playlist
            playlist.Id = 0;
            if (playlist == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", playlistId);
            }

            PlaylistDetailsViewModel articleDetailsViewModel = new PlaylistDetailsViewModel()
            {
                Playlist = playlist
            };
            return View(articleDetailsViewModel);
        }

        [HttpGet]
        [ActionName(nameof(CreatePlaylist))]
        public ViewResult CreatePlaylist()
        {
            var playlists = _databaseRepository.GetAllPlaylists();

            double maxNumber;
            if (playlists.Any())
            {
                maxNumber = (from Playlist p in playlists
                             select p.Rank).Max();
            }
            else
            {
                maxNumber = 0;
            }
            ViewBag.MaxRank = maxNumber;
            return View();
        }

        [HttpPost]
        [ActionName(nameof(CreatePlaylist))]
        public IActionResult CreatePlaylist(PlaylistCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);
                Playlist playlist = new Playlist
                {
                    Title = model.Title,
                    PlaylistBody = model.PlaylistBody,
                    PhotoPath = uniqueFileName,
                    Rank = model.Rank
                };
                _databaseRepository.AddPlaylist(playlist);
                return RedirectToAction("DetailsPlaylist", new { id = protector.Protect(playlist.Id.ToString()) });
            }

            return View();
        }

        [HttpGet]
        [ActionName("EditPlaylist")]
        public ViewResult EditPlaylist(string id)
        {
            int playlistId = Convert.ToInt32(protector.Unprotect(id));
            Playlist playlist = _databaseRepository.GetPlaylist(playlistId);
            PlaylistEditViewModel employeeEditViewModel = new PlaylistEditViewModel()
            {
                EncryptedId = id,
                Title = playlist.Title,
                PlaylistBody = playlist.PlaylistBody,
                ExistingPhotoPath = playlist.PhotoPath,
                Rank = playlist.Rank
            };
            return View(employeeEditViewModel);
        }

        [HttpPost]
        [ActionName("EditPlaylist")]
        public IActionResult EditPlaylist(PlaylistEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Playlist playlist = _databaseRepository.GetPlaylist(Convert.ToInt32(protector.Unprotect(model.EncryptedId)));
                playlist.Title = model.Title;
                playlist.PlaylistBody = model.PlaylistBody;
                playlist.Rank = model.Rank;

                if (model.Photo != null)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath,
                            "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    playlist.PhotoPath = ProcessUploadedFile(model);
                }

                Playlist updatedPlaylist = _databaseRepository.UpdatePlaylist(playlist);

                return RedirectToAction("ListEditPlaylists");
            }

            return View(model);
        }

        [HttpPost]
        [ActionName("DeletePlaylist")]
        [Authorize(Policy = "AdminLevelPolicy")]
        public IActionResult DeletePlaylist(string id)
        {
            if(id.Equals(null))
            {
                throw new Exception();
            }
            int playlistId = Convert.ToInt32(protector.Unprotect(id));
            _databaseRepository.DeletePlaylist(playlistId);
            return RedirectToAction("ListPlaylists");
        }

        private string ProcessUploadedFile(PlaylistCreateViewModel model)
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

