using System.ComponentModel.DataAnnotations;

namespace LMS_library.Data
{
    public class Exam
    {
        public enum ExamType
        {
            Selected = 1,
            Contructed = 2,

        }
        public enum ExamStatus
        {
            Pendding = 0,
            Draft = 1,
            Approved = 2,
            Reject = 3,
        }

        [Key]
        public int id { get; set; }
        [Required]
        public string fileType { get; set; } = string.Empty;
        [Required]
        public string fileName { get; set; } = string.Empty;
        [Required] 
        public string filePath { get; set; } = string.Empty;    
        [Required]
        public string courseName { get; set; } = string.Empty;
        [Required]
        public string teacherEmail { get; set; } = string.Empty;
        [Required]
        public ExamType examType { get; set; } 
        [Required]
        public string time { get; set; } = string.Empty;

        [Required]
        public ExamStatus examStatus { get; set; }
        public DateTime create_At { get; set; } 




    }
}
