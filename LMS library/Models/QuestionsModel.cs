using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static LMS_library.Data.MultipleChoiceQuestions;

namespace LMS_library.Models
{
    public class QuestionsModel
    {
        public int id { get; set; }
        [Required]
        [DefaultValue("Easy")]
        public string difficultLevel { get; set; }
        [Required]
        public string courseName { get; set; }
        [Required]
        [EmailAddress]
        public string teacherEmail { get; set; }
        [Required]
        public string questionName { get; set; }
        [Required]
        public string answerA { get; set; }
        [Required]
        public string answerB { get; set; }
        [Required]
        public string answerC { get; set; }
        [Required]
        public string answerD { get; set; }
        [Required]
        public string correctAnswer { get; set; }
        public DateTime update_At { get; set; } = DateTime.Now;
    }
}
