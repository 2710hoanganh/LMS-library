using System.ComponentModel.DataAnnotations;

namespace LMS_library.Models
{
    public class RoleModel
    {

        [Key]
        public int id { get; set; }
        [Required]
        public string name { get; set; } = string.Empty;
        public string description { get; set; }= string.Empty;
        public DateTime create_At { get; set; } 
        public DateTime update_At { get; set; } = DateTime.Now;
    }
}
