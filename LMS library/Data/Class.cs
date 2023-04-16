using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_library.Data
{
    public class Class
    {
        [Key]
        public int id { get; set; }
        public string classCode { get; set; } = string.Empty;

        public string className { get; set; } = string.Empty;
        public string teacherEmail { get; set; } = string.Empty;
        public int courseId { get; set; }
        [ForeignKey("courseId")]
        public Course Course { get; set; }
        public ICollection<User> Users { get; set; }

    }
}
