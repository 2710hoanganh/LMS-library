using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_library.Data
{
    public class User
    {
        public enum Sex
        {
            None = 0,
            Male =1, 
            Female =2,
        }


        [Key] 
        public int id { get; set; }
        [Required]
        public string userCode { get; set; } = string.Empty;
   
        [Required]
        public string email { get; set; } = string.Empty;
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string? image { get; set; }= string.Empty;

        public Sex sex { get; set; }
        public int phone { get; set; }
        public string address { get; set; } = string.Empty;
        public string passwordHash { get; set; } = string.Empty;
        public string passwordSalt { get; set; } = string.Empty;
        public int? classId { get; set; }
        [ForeignKey("classId")]
        public Class? Class { get; set; }
        public int roleId { get; set; }
        [ForeignKey("roleId")]
        public Role Role { get; set; }

        public string? resetToken { get; set; }  
        public DateTime? resetTokenExpires { get; set; }
        public virtual ICollection<PrivateFiles> files { get; set; }
        public ICollection<Course> Course { get; set; }
        public virtual ICollection<CourseMaterial> materials { get; set; }
    }
}
