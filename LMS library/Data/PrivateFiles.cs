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
        public string fileName { get; set; } = string.Empty;
        [Required]
        public string fileType { get; set; }= string.Empty;
        [Required]
        public string fileSize { get; set; }= string.Empty;
        [Required]
        public string filePath { get; set; } = string.Empty;
        public DateTime uploadAt { get; set; }
        public DateTime updateAt { get; set; }
        public int userId { get; set; }
        [ForeignKey("userId")]
        public User User { get; set; }
    }
    
}
