using System;
namespace ASPNET_Project_Eleven.ViewModels
{
    public class BluePrintEditViewModel : BlueprintCreateViewModel
    {
        public Guid Id { get; set; }
        public string ExistingPhotoPath { get; set; }
    }
}

