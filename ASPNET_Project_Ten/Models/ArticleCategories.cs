using System;
using System.ComponentModel.DataAnnotations;

namespace ASPNET_Project_Eleven.Models
{
    public enum ArticleCategories
    {
        [Display(Name = "None")]
        None,
        [Display(Name = "Amateur Philosophy")]
        AmateurPhilosophy,
        [Display(Name = "Health and Fitness")]
        HealthAndFitness,
        [Display(Name = "Life Improvement")]
        LifeImprovement,
        [Display(Name = "Side Topic")]
        SideTopic
    }
}

