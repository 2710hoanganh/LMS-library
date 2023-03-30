using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace LMS_library.Data
{
    public class Lesson
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string name { get; set; } = string.Empty;
        [Required]
        public string description { get; set; } = string.Empty;
        public int topicId { get; set; }
        [ForeignKey("topicId")]
        public Topic topic { get; set; }
        public int materialId { get; set; }
        public  CourseMaterial Material { get; set; }
        public ICollection<ResourceList> resourceLists { get; set; }    


    }
}
