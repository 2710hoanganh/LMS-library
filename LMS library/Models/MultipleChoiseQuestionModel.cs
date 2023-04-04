using System.ComponentModel.DataAnnotations;

namespace LMS_library.Models
{
    public class MultipleChoiseQuestionModel
    {
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
    }
}
