using System.ComponentModel.DataAnnotations;

namespace LMS_library.Data
{
    public class MultipleChoiceQuestions
    {
        public enum DifficultLevel
        {
            Easy = 0,
            Nomal =1  ,
            Hard =2 ,
        }

        [Key]
        public int id { get; set; }
        public DifficultLevel difficultLevel { get; set; }
        public string courseName { get; set; }
        public string questionName { get; set; }
        public string answerA { get; set; }
        public string answerB { get; set; }
        public string answerC { get; set; }
        public string answerD { get; set; }
        public string correctAnswer { get; set; }
        public DateTime create_At { get; set; } = DateTime.Now;
        public DateTime update_At { get; set;} = DateTime.Now;
    }
}
