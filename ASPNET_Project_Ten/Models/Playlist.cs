using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPNET_Project_Eleven.Models
{
    public class Playlist
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        public string Title { get; set; }
        public string PlaylistBody { get; set; }
        public string PhotoPath { get; set; }
        public double Rank { get; set; }
    }
}

