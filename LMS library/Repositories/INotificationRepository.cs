using Microsoft.AspNetCore.Mvc;

namespace LMS_library.Repositories
{
    public interface INotificationRepository
    {
        public Task AddNotification(string message, int userId, bool IsRead);
        public Task<IEnumerable<Notification>> GetAllById();// get all user notification
        public Task<NotificationModel> GetById(int id);
        public Task UpdateNotificationAsync(int id, NotificationModel model);
        public Task DeleteNotificationAsync(int id);
        public Task<List<Notification>> Filter(string? filter);
    }
}
