using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LMS_library.Models
{

    public class UserRegisterRequest
    {
        [Required]
        public string userCode { get; set; } = string.Empty;
        [Required,EmailAddress]
        public string email { get; set; } = string.Empty;
        public string sex { get; set; }
        [DefaultValue(0123456789)]
        public int phone { get; set; }
        [DefaultValue("Alta soft ware")]
        public string address { get; set; }
        [Required, MinLength(6, ErrorMessage = "Please enter at least 6 character"), MaxLength(25)]
        public string password { get; set; } = string.Empty;
        [Required ,Compare("password")]
        public string confirmPassword { get; set; } = string.Empty;
        [DefaultValue("Student")]
        public string role { get; set; } = string.Empty;
    }
}
