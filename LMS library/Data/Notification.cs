using Org.BouncyCastle.Bcpg;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LMS_library.Data
{
    public class Notification
    {
        [Key]
        public int id { get; set; }
        public string message { get; set; }
        public int userId { get; set; }
        public bool isRead { get; set; }
        public DateTime create_At { get; set; }

    }
}
