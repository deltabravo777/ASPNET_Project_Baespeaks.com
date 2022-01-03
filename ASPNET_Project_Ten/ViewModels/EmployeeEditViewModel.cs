using System;
namespace ASPNET_Project_Eleven.ViewModels
{
    public class EmployeeEditViewModel : EmployeeCreateViewModel
    {
        public string EncryptedId { get; set; }
        public string ExistingPhotoPath { get; set; }
    }
}

