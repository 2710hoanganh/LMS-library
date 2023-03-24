using AutoMapper;
using LMS_library.Data;
using LMS_library.Repositories;

namespace LMS_library.Helpers
{
    public class ApplicationMapper : Profile
    {

        public ApplicationMapper() 
        {
            CreateMap<User,UserModel>().ReverseMap();
            CreateMap<User, UserEditModel>().ReverseMap();
            CreateMap<User, Password>().ReverseMap();
            CreateMap<Role, RoleModel>().ReverseMap();
            CreateMap< SystemDetail , SystemModel>().ReverseMap();
            CreateMap<PrivateFiles, PrivateFileModel>().ReverseMap();
            CreateMap<MaterialType, MaterialTypeModel>().ReverseMap();
            CreateMap<Course , CourseModel>().ReverseMap();
            CreateMap<CourseMaterial, CourseMaterialModel>().ReverseMap();
            CreateMap<MaterialTopic, MaterialTopicModel>().ReverseMap();
        }
    }
}
