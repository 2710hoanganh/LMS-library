using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace LMS_library.Data
{
    public class ResourceList
    {
        [Key]
        public int id { get; set; }
        [Required]
        public int lessonId { get; set; }
        public Lesson Lesson { get; set; }
        public ICollection<CourseMaterial> CourseMaterial { get; set; }

    }
}
