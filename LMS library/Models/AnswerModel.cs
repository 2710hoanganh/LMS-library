using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_library.Models
{
    public class AnswerModel
    {
        public int id { get; set; }
        public int userId { get; set; }
        public int questionId { get; set; }
        public string answer { get; set; } = string.Empty;
    }
}
