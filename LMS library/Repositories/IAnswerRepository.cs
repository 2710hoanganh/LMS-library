namespace LMS_library.Repositories
{
    public interface IAnswerRepository
    {
        public Task<string> AnswerQuestion(AnswerModel model); 
    }
}
