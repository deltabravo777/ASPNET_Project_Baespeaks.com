using System;
namespace ASPNET_Project_Eleven.ViewModels
{
    public class ArticleEditViewModel : ArticleCreateViewModel
    {
        public string EncryptedId { get; set; }
        public string ExistingPhotoPath { get; set; }
    }
}

