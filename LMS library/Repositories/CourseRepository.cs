﻿using AutoMapper;
using System.Security.Claims;

namespace LMS_library.Repositories
{

    public class CourseRepository : ICourseRepository
    {

        private readonly IMapper _mapper;
        private readonly DataDBContex _contex;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CourseRepository(IMapper mapper, DataDBContex contex, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _contex = contex;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<string> AddCourseAsync(CourseModel model)
        {
            var teacher = await _contex.Users.FirstOrDefaultAsync(u => u.email == model.teacherEmail);
            if (teacher == null) { return ("User not existing") ; }
            var role = await _contex.Roles.FirstOrDefaultAsync(r => r.id == teacher.roleId);
            if( role == null || role.name != "Teacher") { return ("Required Teacher ");  }
            var course = new Course
            {
                courseCode = model.courseCode,
                courseName = model.courseName,
                userId = teacher.id,
                description = model.description,
                submission = DateTime.Now,
   
                pendingMaterial = model.pendingMaterial,
                createDate = DateTime.Now,
            };


            var newCourse = _mapper.Map<Course>(course);
            _contex.Courses.Add(newCourse);
            await _contex.SaveChangesAsync();
            return ("create successfully .");
        }

        public async Task DeleteCourseAsync(int id)//course id
        {
            var delete = await _contex.Courses!.FindAsync(id);
            if (delete != null)
            {
                _contex.Courses.Remove(delete);
                await _contex.SaveChangesAsync();
            }
        }

        public async Task<List<Course>> GetAll()
        {
            var course = await _contex.Courses!.ToListAsync();
            return _mapper.Map<List<Course>>(course);
        }

        public async Task<List<CourseModel>> GetAllForTeacher()
        {
            var result = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Email);
            var user = await _contex.Users!.FirstOrDefaultAsync(u => u.email == result.ToString());
            var courses = await _contex.Courses!.Where(c => c.userId == user.id).ToListAsync();
            
            return _mapper.Map<List<CourseModel>>(courses);
        }
        public async Task<Course> GetById(int id) //course id
        {
            var course = await _contex.Courses!.FindAsync(id);
            return _mapper.Map<Course>(course);
        }

        public async Task UpdateCourseAsync(int id, CourseModel model) //course id
        {
            if (id == model.id)
            {
                var teacher = await _contex.Users.FirstOrDefaultAsync(u => u.email == model.teacherEmail);
                var course = await _contex.Courses!.FindAsync(model.id);
                if(course== null) { return; }
                course.courseCode = model.courseCode;
                course.courseName = model.courseName;
                course.userId = teacher.id;
                course.description = model.description;
                course.submission = course.submission;
                course.pendingMaterial = course.pendingMaterial;
                course.createDate = course.createDate;
   
                var updateCourse = _mapper.Map<Course>(course);
                _contex.Courses?.Update(updateCourse);
                await _contex.SaveChangesAsync();
            }
        }
        

        public async Task StudentSetLikedCourse(int id)
        {
            var result = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var course = await _contex.Courses!.FindAsync(id);
            if(course == null)
            {
                return;
            }
            var classInfo = await _contex.Classes.FirstOrDefaultAsync(c => c.courseId == course.id);
            if(classInfo== null || classInfo.id != Int32.Parse(result)) { return; }
            course.courseCode = course.courseCode;
            course.courseName = course.courseName;
            course.userId = course.userId;
            course.description = course.description;
            course.submission = course.submission;
            course.pendingMaterial = course.pendingMaterial;
            course.createDate = course.createDate;
            var updateCourse = _mapper.Map<Course>(course);
            _contex.Courses?.Update(updateCourse);
            await _contex.SaveChangesAsync();

        }   
        public async Task<List<CourseModel>> SearchSort(string? search, string? course, string? teacher, string? courseCode)
        {
            var courses = _contex.Courses.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                courses = _contex.Courses.Where(m => m.courseName.Contains(search));
            }
            if (!string.IsNullOrEmpty(course))
            {
                courses = _contex.Courses.Where(m => m.courseName.Contains(search));
            }
            if (!string.IsNullOrEmpty(teacher))
            {
                courses = _contex.Courses.Where(m => m.User.email.Contains(teacher));
            }
            if (!string.IsNullOrEmpty(courseCode))
            {
                courses = _contex.Courses.Where(m => m.courseCode.Contains(courseCode));
            }



            var result = courses.Select(r => new CourseModel
            {
                id = r.id,
                courseCode = r.courseCode,
                courseName = r.courseName,
                teacherEmail =r.User.email,
                description =r.description,
                submission = r.submission,
                createDate = r.createDate
            });
            return result.ToList(); 
        }

        public async Task<List<CourseModel>> Fillter()
        {
            var courses = _contex.Courses.OrderBy(c => c.courseName);


            var result = courses.Select(r => new CourseModel
            {
                id = r.id,
                courseCode = r.courseCode,
                courseName = r.courseName,
                teacherEmail = r.User.email,
                description = r.description,
                submission = r.submission,
                createDate = r.createDate
            });
            return result.ToList();
        }

    }
}
