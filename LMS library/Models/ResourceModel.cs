using System.ComponentModel.DataAnnotations;

namespace LMS_library.Models
{
    public class ResourceModel
    {
        public int id { get; set; }
        public string courseName { get; set; }
        public string topicName { get; set; }
        public string lessonName { get; set; }
        public string materialName { get; set; }
    }
}
