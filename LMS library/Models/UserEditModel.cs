using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LMS_library.Models
{
    public class UserEditModel
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string userCode { get; set; }
        [Required, EmailAddress]
        public string email { get; set; } = string.Empty;
        [DefaultValue("First Name")]
        public string firstName { get; set; } = null!;
        [DefaultValue("Last Name")]
        public string lastName { get; set; } = null!;
        public string role { get; set; } = string.Empty;
    }
}
