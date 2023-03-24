using System.ComponentModel.DataAnnotations;

namespace LMS_library.Models
{
    public class MaterialTypeModel
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string name { get; set; } = string.Empty;
    }
}
