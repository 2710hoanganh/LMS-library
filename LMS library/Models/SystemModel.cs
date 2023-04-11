using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LMS_library.Models
{
    public class SystemModel
    {
        [Key]
        public int id { get; set; }
        [DefaultValue("School Name")]
        public string schoolName { get; set; } = string.Empty;
        [DefaultValue("SchoolWebSite.com")]
        public string schoolWebSite { get; set; } = string.Empty;
        [DefaultValue("School Type")]
        public string shoolType { get; set; } = string.Empty;
        [DefaultValue("Principal Name")]
        public string principal { get; set; } = string.Empty;
        [DefaultValue("Library Name")]
        public string libraryName { get; set; } = string.Empty;
        [DefaultValue("Library.com")]

        public string lybraryWebSite { get; set; } = string.Empty;
        [DefaultValue(0931872335)]

        public int lybraryPhone { get; set; }
        [DefaultValue("example@example.com") ,EmailAddress]
        public string lybraryEmail { get; set; } = string.Empty;
        public string image { get; set; } = string.Empty;
    }
}
