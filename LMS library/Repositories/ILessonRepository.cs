namespace LMS_library.Repositories
{
    public interface ILessonRepository
    {
        public Task<List<Lesson>> GetAll();
        public Task<List<Lesson>> GetAllOnTopic(int id);
        public Task<LessonModel> GetById(int id);
        public Task<string> AddLessonAsync(LessonModel model);
        public Task UpdateLessonAsync(int id, LessonModel model);
        public Task DeleteLessonAsync(int id);
    }
}
