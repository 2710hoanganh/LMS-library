using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_library.Data
{
    public class SystemDetail
    {
        [Key]
        public int id { get; set; }
        public string schoolName { get; set; } =string.Empty;
        public string schoolWebSite { get; set; } = string.Empty;
        public string shoolType { get; set; } = string.Empty;
        public string principal { get; set; } = string.Empty;
        public string libraryName { get; set; } = string.Empty;

        public string lybraryWebSite { get; set; } = string.Empty;

        public int lybraryPhone { get; set; } 
        public string lybraryEmail { get; set; } = string.Empty;
        public string? image { get; set; } = string.Empty;

    }
}
