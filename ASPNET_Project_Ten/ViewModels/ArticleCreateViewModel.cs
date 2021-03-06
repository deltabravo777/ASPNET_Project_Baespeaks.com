using System;
using System.ComponentModel.DataAnnotations;
using ASPNET_Project_Eleven.Models;
using Microsoft.AspNetCore.Http;

namespace ASPNET_Project_Eleven.ViewModels
{
    public class ArticleCreateViewModel
    {
        [Required]
        public string TitleName { get; set; }
        public string ArticleBody { get; set; }
        public IFormFile Photo { get; set; }
        public int Year { get; set; }
        public CalendarMonths Month { get; set; }
        public int Day { get; set; }
        public double Rank { get; set; }
        public ArticleCategories Category { get; set; }
    }
}

