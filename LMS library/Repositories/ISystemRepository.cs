namespace LMS_library.Repositories
{
    public interface ISystemRepository
    {
        public Task<SystemModel> GetById(int id);
        public Task<string> AddDetailAsync(SystemModel model);
        public Task UpdateDetaiAsync(int id, SystemModel model);
        public Task DeleteDetailAsync(int id);
    }
}
