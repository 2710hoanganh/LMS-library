namespace LMS_library.Models
{
    public class ClassModel
    {
        public int id { get; set; }
        public string classCode { get; set; } = string.Empty;

        public string className { get; set; } = string.Empty;
        public string teacherEmail { get; set; } = string.Empty;
        public int courseId { get; set; }
    }
}
