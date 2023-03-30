using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace LMS_library.Data
{
    public class Topic
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string name { get; set; } = string.Empty;

        public string description { get; set; }= string.Empty;
        public int courseId { get; set; }
        [ForeignKey("courseId")]
        public Course courses { get; set; }
        public ICollection<Lesson> Lessons { get; set; }    
    }
}