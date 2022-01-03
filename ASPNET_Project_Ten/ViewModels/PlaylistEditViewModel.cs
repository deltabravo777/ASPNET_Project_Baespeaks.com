using System;
namespace ASPNET_Project_Eleven.ViewModels
{
    public class PlaylistEditViewModel : PlaylistCreateViewModel
    {
        public string EncryptedId { get; set; }
        public string ExistingPhotoPath { get; set; }
    }
}

