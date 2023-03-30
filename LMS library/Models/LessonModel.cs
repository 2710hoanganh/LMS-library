using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMS_library.Models
{
    public class LessonModel
    {
        public int id { get; set; }
        [Required]
        public string name { get; set; } = string.Empty;
        [Required]
        public string description { get; set; } = string.Empty;
        public string topicName { get; set; }
        public string materialName { get; set; }
    }
}
