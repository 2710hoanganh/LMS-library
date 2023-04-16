using AutoMapper;
using System.Security.Claims;

namespace LMS_library.Repositories
{
    public class SentHelpRepository : ISentHelpRepository
    {
        private readonly IMapper _mapper;
        private readonly DataDBContex _contex;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SentHelpRepository(IMapper mapper, DataDBContex contex, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _contex = contex;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> SentHelp(SentHelpModel model)
        {
            var help = new SendHelp
            {
                userEmail = UserInfo(),
                content = model.content,
            };
            var helpTicket = _mapper.Map<SendHelp>(help);
            _contex.SendHelps.Add(helpTicket);
            await _contex.SaveChangesAsync();
            return ("Sent Help Successfully !");
        }
        private string UserInfo()
        {
            var result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            return result;
        }
    }
}
