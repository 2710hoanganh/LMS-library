using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic.FileIO;

namespace LMS_library.Data
{
    public class PrivateFiles
    {

        [Key]
        public int id { get; set; }
        [Required]
        public string fileName { get; set; }
        [Required]
        public string fileType { get; set; }
        [Required]
        public string fileSize { get; set; }
        [Required]
        public string filePath { get; set; } 
        public DateTime uploadAt { get; set; }
        public DateTime updateAt { get; set; }
        public int userId { get; set; }
        [ForeignKey("userId")]
        public User User { get; set; }
    }
    
}
