using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using LMS_library.Data;
using System.Security.Claims;

namespace LMS_library.Repositories
{
    public class ClassRepository : IClassRepository
    {


        private readonly IMapper _mapper;
        private readonly DataDBContex _contex;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        public ClassRepository(IUserRepository userRepository,IMapper mapper, DataDBContex contex, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _contex = contex;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }
        public async Task<string> AddClassAsync(ClassModel model)
        {
            var checkClass = await _contex.Classes!.FirstOrDefaultAsync(c => c.className == model.className || c.classCode == model.classCode);
            if (checkClass != null) { return ("Class Already Exist !"); }
            var course = await _contex.Courses!.FirstOrDefaultAsync(c => c.id == model.courseId && c.User.email == model.teacherEmail);
            if (course == null) { return ("Course Not Even Exist ! "); }
            var newClass = new Class
            {
                classCode = model.classCode,
                className = model.className,
                teacherEmail =model.teacherEmail,
                courseId = course.id,
            };
            var createClass = _mapper.Map<Class>(newClass);
            _contex.Classes.Add(newClass);
            await _contex.SaveChangesAsync();
            return ("Create Class Successfully !");
        }

        public async Task DeleteClassAsync(int id)
        {
            var delete = await _contex.Classes!.FindAsync(id);
            if (delete == null) { return; }
            _contex.Classes.Remove(delete);
            await _contex.SaveChangesAsync();
        }

        public async Task<List<Class>> GetAllBaseOnCourse(int id)
        {
            var classes = await _contex.Classes!.Where(c => c.courseId == id).ToListAsync();
            return _mapper.Map<List<Class>>(classes);
        }

        public async Task<List<Class>> GetAllClassBaseOnTeacher(string teacher)
        {
            var classes = await _contex.Classes!.Where(c => c.teacherEmail == teacher).ToListAsync();
            return _mapper.Map<List<Class>>(classes);
        }



        public async Task<List<Class>> GetAll()
        {
            var classes = await _contex.Classes!.ToListAsync();
            return _mapper.Map<List<Class>>(classes);
        }


        public async Task<Class> GetById(int id)
        {
            var findClass = await _contex.Classes!.FindAsync(id);
            return _mapper.Map<Class>(findClass);
        }


        public async Task UpdateClassAsync(int id, ClassModel model)
        {
            if (id == model.id) 
            {
                var findClass = await _contex.Classes.FindAsync(id);
                var course = await _contex.Courses.FindAsync(model.id);
                var checkTeacher = await _contex.Classes!.FirstOrDefaultAsync(t => t.teacherEmail == model.teacherEmail);
                if (checkTeacher != null || findClass == null || course == null) { return ; }

                findClass.classCode= model.classCode;
                findClass.className = model.className;
                findClass.teacherEmail= model.teacherEmail;
                findClass.courseId = model.courseId;

                var updateClass = _mapper.Map<Class>(findClass);
                _contex.Classes?.Update(updateClass);
                await _contex.SaveChangesAsync();
            }
        }


        private string UserInfo()
        {
            var result = _httpContextAccessor!.HttpContext!.User.FindFirstValue(ClaimTypes.Email);
            return result;
        }
    }
}
    