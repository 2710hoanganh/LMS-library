using static LMS_library.Data.CourseMaterial;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMS_library.Models
{
    public class CourseMaterialModel
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string materialType { get; set; }
        [Required]
        public string teacherEmail { get; set; }
        [Required]
        public string courseName { get; set; } = string.Empty;
        public string lessonName { get; set; }= string.Empty;
        public FileStatus fileStatus { get; set; }
        public string materialPath { get; set; }
        public int fileSize { get; set; }
        public DateTime submission_date { get; set; } = DateTime.Now;

    }
}
