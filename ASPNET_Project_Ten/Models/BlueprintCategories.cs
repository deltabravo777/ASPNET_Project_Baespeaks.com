using System;
using System.ComponentModel.DataAnnotations;
namespace ASPNET_Project_Eleven.Models
{
    public enum BlueprintCategories
    {
        [Display(Name = "None v2")]
        None,
        [Display(Name = "C# Code")]
        Csharp,
        [Display(Name = "ASP.NET Core")]
        ASPNET
    }
}