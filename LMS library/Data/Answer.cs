using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_library.Data
{
    public class Answer
    {
        [Key]
        public int id { get; set; }
        public int userId { get; set; }
        public int questionId { get; set; }
        [ForeignKey("questionId")]
        public Question Question { get; set; }
        public string answer { get; set; } = string.Empty;
    }
}
