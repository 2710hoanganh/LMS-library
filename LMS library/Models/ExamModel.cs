using System.ComponentModel.DataAnnotations;

namespace LMS_library.Models
{
    public class ExamModel
    {
        public int id { get; set; }
        public string fileType { get; set; } = string.Empty;
        public string fileName { get; set; } = string.Empty;
        public string filePath { get; set; } = string.Empty;
        public string courseName { get; set; } = string.Empty;
        public string teacherEmail = string.Empty;
        public int  examType { get; set; }
        public string time { get; set; } = string.Empty;

        public int examStatus { get; set; }
        public DateTime create_At { get; set; }= DateTime.Now;
    }
}
