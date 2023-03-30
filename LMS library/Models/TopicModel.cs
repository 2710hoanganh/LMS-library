using System.ComponentModel.DataAnnotations;

namespace LMS_library.Models
{
    public class TopicModel
    {
        public int id { get; set; }
        [Required]
        public string name { get; set; }

        public string description { get; set; }
        public string courseName { get; set; } 
    }
}
