using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace LMS_library.Data
{
    public class CourseMaterial
    {
        public enum FileStatus
        {
            Pendding = 0 ,
            Approved =1 ,
            Reject = 2 ,
        }
        [Key]
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        [Required] 
        public int materialTypeID { get;set; }
        [ForeignKey("materialTypeID")]
        public MaterialType MaterialType { get; set; }
        [Required]
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int? courseId { get; set; }
        [ForeignKey("courseId")]

        public Course courses { get; set; }
        public FileStatus fileStatus { get; set; }
        public string materialPath { get; set; }
        public int fileSize { get; set; }

        public DateTime submission_date { get; set; }
        public ResourceList ResourceList { get; set; }
        public  Lesson Lesson { get; set; }



    }
}
