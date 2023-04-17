using AutoMapper;
using System.Security.Claims;

namespace LMS_library.Repositories
{
    public class AnswerRepository :IAnswerRepository
    {

        private IMapper _mapper;
        private DataDBContex _contex;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AnswerRepository(DataDBContex contex , IHttpContextAccessor httpContextAccessor ,IMapper mapper) 
        {
            _contex = contex;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;

        }

        public async Task<string> AnswerQuestion( AnswerModel model) // question id
        {
            var question = await _contex.LessonQuestions!.FirstOrDefaultAsync(q => q.id == model.questionId);
            if (question == null) { return ("Question Not Existing !"); }
            var answer = new Answer
            {
                userId = Int32.Parse(UserInfo()),
                questionId = model.questionId,
                answer = model.answer
            };
            var newAnswer= _mapper.Map<Answer>(answer);

            _contex.Answers.Add(newAnswer);
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
