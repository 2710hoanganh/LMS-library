namespace LMS_library.Repositories
{
    public interface ILessonQuestionRepository
    {
        public Task<string> AddQuestion(LessonQuestionModel model);
    }
}
