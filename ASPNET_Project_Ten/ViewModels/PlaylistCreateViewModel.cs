using System;
using System.ComponentModel.DataAnnotations;
using ASPNET_Project_Eleven.Models;
using Microsoft.AspNetCore.Http;

namespace ASPNET_Project_Eleven.ViewModels
{
    public class PlaylistCreateViewModel
    {
        [Required]
        public string Title { get; set; }
        public string PlaylistBody { get; set; }
        public IFormFile Photo { get; set; }
        public double Rank { get; set; }
    }
}

