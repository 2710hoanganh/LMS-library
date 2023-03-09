using System.ComponentModel.DataAnnotations;

namespace LMS_library.Models
{
    public class UserModel
    {
        [Key]
        public int id { get; set; }
        [Required,EmailAddress]
        public string email { get; set; } = string.Empty;
        [Required, MinLength(6, ErrorMessage = "Please enter at least 6 character"), MaxLength(25)]
        public string password { get; set; } = string.Empty;
        [Required, Compare("password")]
        public string confirmPassword { get; set; } = string.Empty;
    }
}
