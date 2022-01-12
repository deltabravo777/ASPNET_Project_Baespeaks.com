using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPNET_Project_Eleven.Models
{
    public class Blueprint
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string BlueprintBody { get; set; }
        public string PhotoPath { get; set; }

        public int Year { get; set; }
        public CalendarMonths Month { get; set; }
        public int Day { get; set; }
        public double Rank { get; set; }
        public BlueprintCategories Category { get; set; }
    }
}

