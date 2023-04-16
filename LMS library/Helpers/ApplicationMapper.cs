using AutoMapper;
using LMS_library.Data;
using LMS_library.Models;

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
            CreateMap<Topic, TopicModel>().ReverseMap();
            CreateMap<Lesson, LessonModel>().ReverseMap();
            CreateMap<ResourceList, ResourceModel>().ReverseMap();
            CreateMap<Exam, ExamModel>().ReverseMap();
            CreateMap<MultipleChoiceQuestions, QuestionsModel>().ReverseMap();
            CreateMap<Notification, NotificationModel>().ReverseMap();
            CreateMap<SendHelp, SentHelpModel>().ReverseMap();
            CreateMap<Class, ClassModel>().ReverseMap();
        }
    }
}
