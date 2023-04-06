using Microsoft.AspNetCore.Mvc;

namespace LMS_library.Repositories
{
    public interface INotificationRepository
    {
        public Task<List<Notification>> GetAllById();// get all user notification
        public Task<NotificationModel> GetById(int id);
        public Task UpdateNotificationAsync(int id, NotificationModel model);
        public Task DeleteNotificationAsync(int id);
        public Task<List<Notification>> Filter(string? filter);
    }
}
