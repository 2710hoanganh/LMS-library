using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http.Headers;

namespace LMS_library.Data
{
    public class Question
    {
        [Key]
        public int id { get; set; }
        public int userId { get; set; }
        public int lessonId { get; set; }
        [ForeignKey("lessonId")]
        public Lesson Lesson { get; set; }
        public string title { get; set; }   = string.Empty;
        public string content { get; set; } = string.Empty;
        public virtual ICollection<Answer> Answers { get; set; }


    }
}
