using System.ComponentModel.DataAnnotations;

namespace LMS_library.Data
{
    public class Role
    {

        [Key]
        public int id { get; set; }
        [Required]
        public string name { get; set; } = string.Empty;
        public DateTime create_At { get; set; } = DateTime.Now;
        public DateTime update_At { get; set; } 
    }
}
