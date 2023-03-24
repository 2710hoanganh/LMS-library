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
        [Required]
        public int userId { get; set; }
        [ForeignKey("userId")]
        public int? courseId { get; set; }
        [ForeignKey("courseId")]
        public int? materialTopicId { get; set; }
        [ForeignKey("materialTopicId")]
        public FileStatus fileStatus { get; set; }
        public string materialPath { get; set; }
        public int fileSize { get; set; }

        public DateTime submission_date { get; set; }
        public MaterialType MaterialType { get; set; }

        public Course courses { get; set; }
        public MaterialTopic MaterialTopic { get; set; }


    }
}
