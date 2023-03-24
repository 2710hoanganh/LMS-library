using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace LMS_library.Models
{
    public class CourseModel
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string courseCode { get; set; } = string.Empty;
        [Required]
        public string courseName { get; set; } = string.Empty;
        [Required,EmailAddress]
        public string teacherEmail { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public DateTime submission { get; set; } = DateTime.Now;
        [DefaultValue(0)]
        public int pendingMaterial { get; set; }
        public DateTime createDate { get; set; } = DateTime.Now;
    }
}
