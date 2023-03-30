﻿namespace LMS_library.Repositories
{
    public interface IExamRepository
    {
        public Task<List<Exam>> GetAllForStudent(string name);
        public Task<List<Exam>> GetAll(string name);
        public Task PostWordAsync(string name ,string type ,string time , List<IFormFile> privateFileUploads);
        public Task UpdateFileAsync(string newName, int id);
    }
}
