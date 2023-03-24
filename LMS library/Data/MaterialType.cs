using System.ComponentModel.DataAnnotations;

namespace LMS_library.Data
{
    public class MaterialType
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string name { get; set; } = string.Empty;
        public virtual ICollection<CourseMaterial> CourseMaterial { get; set; }
        MaterialType() { CourseMaterial = new List<CourseMaterial>(); }
    }
}
