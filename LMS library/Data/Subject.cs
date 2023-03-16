using System.ComponentModel.DataAnnotations;

namespace LMS_library.Data
{
    public class Subject
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string subjectCode { get; set; }
        [Required]
        public string subjectName { get; set; } = string.Empty;

        public DateTime createDate { get; set; }
    }
}
