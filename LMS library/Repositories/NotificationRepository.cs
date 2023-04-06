using AutoMapper;
using System.Security.Claims;

namespace LMS_library.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly DataDBContex _contex;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public NotificationRepository(DataDBContex contex , IMapper mapper,IHttpContextAccessor httpContextAccessor) 
        { 
            _contex = contex;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task DeleteNotificationAsync(int id)
        {
            var result = _httpContextAccessor!.HttpContext!.User.FindFirstValue(ClaimTypes.Email);
            var user = await _contex.Users!.FirstOrDefaultAsync(u => u.email == result);
            var delete = await _contex.Notifications!.FindAsync(id);
            if(user.id != delete.userId) { return; }
            _contex.Notifications.Remove(delete);
            await _contex.SaveChangesAsync();
        }

        public Task<List<Notification>> Filter(string? filter)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Notification>> GetAllById()
        {
            var result = _httpContextAccessor!.HttpContext!.User.FindFirstValue(ClaimTypes.Email);
            var user = await _contex.Users!.FirstOrDefaultAsync(u => u.email == result);
            var notifications = await _contex.Notifications!
                .Where(n => n.userId == user.id )
                .ToListAsync();

            return _mapper.Map<List<Notification>>(notifications);
        }

        public async Task<NotificationModel> GetById(int id)
        {
            var notification = await _contex.Notifications!.FindAsync(id);
            notification.isRead = true;
            _contex.Notifications!.Update(notification);
            await _contex.SaveChangesAsync();
            return _mapper.Map<NotificationModel>(notification);
        }

        public Task UpdateNotificationAsync(int id, NotificationModel model)
        {
            throw new NotImplementedException();
        }
    }
}
