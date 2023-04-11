

namespace LMS_library.Repositories
{
    public interface ICourseMaterialRepository
    {
        public Task<List<CourseMaterial>> GetAllForStudent(int id);
       
        public Task<List<CourseMaterial>> GetAll();
        public Task<List<CourseMaterial>> GetAllBaseOnCourse(int id); //base on  course id
        public Task<List<CourseMaterial>> GetAllLesson();//base on teacher id
        public Task<List<CourseMaterial>> GetAllResource();//base on teacher id
        public Task FileApprove(string check , int id);

        public Task UpdateFileAsync(string newName, int id);
        public Task AddToResource(string lessonName, int id);
        public Task DeleteFileAsync(int id);
        public Task PostMultiFileAsync( string type, string course ,List<IFormFile> privateFileUploads);
        public Task<List<CourseMaterialModel>> SearchSort(string? course, string? teacher, string? status);
        public Task<List<CourseMaterialModel>> Fillter();

    }
}
