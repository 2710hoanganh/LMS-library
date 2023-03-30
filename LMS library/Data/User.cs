using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_library.Data
{
    public class User
    {
        [Key] 
        public int id { get; set; }
        [Required]
        public string userCode { get; set; } = string.Empty;
   
        [Required]
        public string email { get; set; } = string.Empty;
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;

        public string passwordHash { get; set; } = string.Empty;
        public string passwordSalt { get; set; } = string.Empty;
  
        public int? roleId { get; set; }
        [ForeignKey("roleId")]
        public Role Role { get; set; }

        public string? resetToken { get; set; }  
        public DateTime? resetTokenExpires { get; set; }

        public virtual ICollection<PrivateFiles> files { get; set; }
        public ICollection<Course> Course { get; set; }
        public virtual ICollection<CourseMaterial> materials { get; set; }
    }
}
