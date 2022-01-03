using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPNET_Project_Eleven.Models
{
    public class Article
    {
        // [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        public string Title { get; set; }
        public string ArticleBody { get; set; }
        public string PhotoPath { get; set; }

        public int Year { get; set; }
        public CalendarMonths Month { get; set; }
        public int Day { get; set; }
        public double Rank { get; set; }
        public ArticleCategories Category { get; set; }
    }
}

