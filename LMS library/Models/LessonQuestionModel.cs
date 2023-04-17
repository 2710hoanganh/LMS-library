using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_library.Models
{
    public class LessonQuestionModel
    {
        public int id { get; set; }
        public int userId { get; set; }
        public string title { get; set; } = string.Empty;
        public string content { get; set; } = string.Empty;
        public string topic { get; set; } = string.Empty;
        public string lesson { get; set; } = string.Empty;
    }
}
