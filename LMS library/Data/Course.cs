using DocumentFormat.OpenXml.Drawing.Charts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_library.Data
{
    public class Course
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string courseCode { get; set; } = string.Empty;
        [Required]
        public string courseName { get; set; } = string.Empty;
        [Required]
        public int  userId { get; set; }
        [ForeignKey("userId")]
        public User User { get; set; }
        public string description { get; set; } = string.Empty;
        public DateTime submission { get; set; }
        public int pendingMaterial { get; set; }
        public DateTime createDate { get; set; } = DateTime.Now;
        public ICollection<Class>? classes { get; set; }
        public ICollection<Topic>? topics { get; set; }
        public ICollection<CourseMaterial>? materials { get; set; }
    }
}
