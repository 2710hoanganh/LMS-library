using LMS_library.Data;
using Microsoft.AspNetCore.Mvc;

namespace LMS_library.Repositories
{
    public interface IQuestionsRepository
    {
        public Task<List<MultipleChoiceQuestions>> GetAll();
        public Task<QuestionsModel> GetById(int id);
        public Task<string> AddQuestionAsync(QuestionsModel model);
        public Task UpdateQuestionAsync(int id, QuestionsModel model);
        public Task DeleteQuestionAsync(int id);
        public Task<List<QuestionsModel>> Search(string? search);
        public  Task<List<QuestionsModel>> Filter(string? course, string? teacher, string? difficult);
        public Task AddExamByExistQuestion (string courseName , string examName,string time , int examNumber ,int pointRange ,int questionQuantity , int easyQuestionQuantity , int nomalQuestionQuantity, int hardQuestionQuantity);
    }
}
