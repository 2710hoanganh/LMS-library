using Microsoft.AspNetCore.Mvc;

namespace LMS_library.Repositories
{
    public interface IClassRepository
    {
        public Task<List<Class>> GetAll();
        public Task<List<Class>> GetAllClassBaseOnTeacher(string teacher);//teacher Email
        public Task<List<Class>> GetAllBaseOnCourse(int id );//class id

        public Task<Class> GetById(int id);
        public Task<string> AddClassAsync(ClassModel model);
        public Task UpdateClassAsync(int id, ClassModel model);
        public Task DeleteClassAsync(int id);
    }
}
