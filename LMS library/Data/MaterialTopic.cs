using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace LMS_library.Data
{
    public class MaterialTopic
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string name { get; set; }

        public string description { get; set; }
        public int courseId { get; set; }
        [ForeignKey("courseId")]
        public Course courses { get; set; }
        public ICollection<CourseMaterial> materials { get; set; }
    }
}