using System.ComponentModel.DataAnnotations;

namespace LMS_library.Data
{
    public class SendHelp
    {
        [Key]
        public int id { get; set; }
        public string userEmail { get; set; } = string .Empty;
        public string content { get; set; } = string.Empty;
    }
}
