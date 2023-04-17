using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using LMS_library.Data;
using System.Security.Claims;

namespace LMS_library.Repositories
{
    public class LessonQuestionRepository : ILessonQuestionRepository
    {
        private DataDBContex _contex;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IMapper _mapper;


        public LessonQuestionRepository(DataDBContex contex , IHttpContextAccessor httpContextAccessor ,IMapper mapper) 
        {
            _contex = contex;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public  async Task<string> AddQuestion(LessonQuestionModel model)
        {
            var toppic = await _contex.Topics!.FirstOrDefaultAsync(t => t.name == model.topic);
            if (toppic == null) { return ("Cant Find Topic"); }
            var lesson = await _contex.Lessons!.FirstOrDefaultAsync(l => l.topicId == toppic.id);
            if(lesson == null || model.title== null || model.content == null) { return ("Error"); }
            var newQuestion = new Question
            {
                userId = Int32.Parse(UserInfo()),
                title = model.title,
                content = model.content,
                lessonId = lesson.id
            };
            var newquestion = _mapper.Map<Question>(newQuestion);

            _contex.LessonQuestions.Add(newquestion);
            await _contex.SaveChangesAsync();
            return ("Sent Question Successfully .");

        }
        private string UserInfo()
        {
            var result = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return result;
        }
    }
}
