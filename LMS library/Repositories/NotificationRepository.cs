using AutoMapper;
using LMS_library.Data;
using LMS_library.Data_Service;
using System.Security.Claims;

namespace LMS_library.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly DataDBContex _contex;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICaching _caching;
        private readonly string cachingKey = "notifications";
        public NotificationRepository(DataDBContex contex , IMapper mapper,IHttpContextAccessor httpContextAccessor , ICaching caching) 
        { 
            _contex = contex;
            _mapper = mapper;
            _caching = caching;
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


        public async Task AddNotification(string message, int userId, bool IsRead)
        {
            var newNoti = new Notification
            {
                message = message,
                userId = userId,
                isRead = IsRead,
            };
            _contex.Notifications.Add(newNoti);
            await _contex.SaveChangesAsync();
        }

        public Task<List<Notification>> Filter(string? filter)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Notification>> GetAllById()
        {
            var cacheData = _caching.GetData<IEnumerable<Notification>>(cachingKey);
            var result = _httpContextAccessor!.HttpContext!.User.FindFirstValue(ClaimTypes.Email);
            var user = await _contex.Users!.FirstOrDefaultAsync(u => u.email == result);
            if (cacheData != null && cacheData.Count() > 0)
            {
                return _mapper.Map<IEnumerable<Notification>>(cacheData.Where(c => c.userId == user.id));
            }
            else
            {
                var expiryTime = DateTimeOffset.Now.AddSeconds(30);
                cacheData = await _contex.Notifications!
                    .Where(n => n.userId == user.id)
                    .ToListAsync();
                _caching.SetData(cachingKey, cacheData, expiryTime);

                return _mapper.Map<List<Notification>>(cacheData);
            }
        }

        public async Task<NotificationModel> GetById(int id)
        {
            var cacheData = _caching.GetData<IEnumerable<Notification>>(cachingKey);
            if (cacheData != null && cacheData.Count() > 0)
            {
                var updateNotify = cacheData.FirstOrDefault(c => c.id == id);
                updateNotify.isRead = true;
                _contex.Notifications!.Update(updateNotify);
                await _contex.SaveChangesAsync();
                return _mapper.Map<NotificationModel>(cacheData.Where(c => c.id == id));
            }
            else
            {
                var notification = await _contex.Notifications!.FindAsync(id);
                notification.isRead = true;
                _contex.Notifications!.Update(notification);
                await _contex.SaveChangesAsync();
                return _mapper.Map<NotificationModel>(notification);
            }
        }

        public Task UpdateNotificationAsync(int id, NotificationModel model)
        {
            throw new NotImplementedException();
        }
    }
}
