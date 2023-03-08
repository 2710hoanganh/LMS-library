using AutoMapper;

namespace LMS_library.Helpers
{
    public class ApplicationMapper : Profile
    {

        public ApplicationMapper() 
        {
            CreateMap<User,UserModel>().ReverseMap(); 
        }
    }
}
