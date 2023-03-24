namespace LMS_library.Repositories
{
    public interface ICourseRepository
    {

        public Task<List<Course>> GetAll();
        public Task<List<CourseModel>> GetAllForTeacher();
        public Task<Course> GetById(int id);
        public Task<string> AddCourseAsync(CourseModel model);
        public Task UpdateCourseAsync(int id, CourseModel model);
        public Task DeleteCourseAsync(int id);
    }
}
