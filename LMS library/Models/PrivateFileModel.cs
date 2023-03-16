
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMS_library.Models
{
    public class PrivateFileModel
    {

        [Key]
        public int id { get; set; }
        [Required]
        public string fileName { get; set; } = string.Empty;
        [Required]
        public string fileType { get; set; } = string.Empty;
        [Required]
        public string fileSize { get; set; } = string.Empty;
        public DateTime uploadAt { get; set; } = DateTime.Now;
        public DateTime updateAt { get; set; } = DateTime.Now;
        [Required]
        public int userId { get; set; }
    }
}
