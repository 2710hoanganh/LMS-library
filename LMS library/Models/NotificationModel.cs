namespace LMS_library.Models
{
    public class NotificationModel
    {
        public string message { get; set; }
        public int userId { get; set; }
        public bool isRead { get; set; }
    }
}
