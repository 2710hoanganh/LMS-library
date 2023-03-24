﻿using AutoMapper;
using System.Security.Claims;

namespace LMS_library.Repositories
{
    public class CourseMaterialRepository :ICourseMaterialRepository
    {
        private readonly DataDBContex _contex;
        private IHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;



        public CourseMaterialRepository(DataDBContex contex, IMapper mapper, IHostEnvironment environment, IHttpContextAccessor httpContextAccessor) 
        {
            _contex = contex;
            _mapper = mapper;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task PostMultiFileAsync( string type , string course ,List<IFormFile> MaterialUploads)// type = material file type (Lesson or Resource) , course = course name
        {
            var result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            if (result == null)
            {
                return;
            }
            int courseID =0 ;
            var user = await _contex.Users.FirstOrDefaultAsync(u => u.email == result);
            var materialType = await _contex.MaterialTypes.FirstOrDefaultAsync(t => t.name == type);
            var courseName = await _contex.Courses.FirstOrDefaultAsync(c => c.courseName== course);
            if(courseName ==null)
            {
                courseID = 0;
            }

            if(courseName != null )
            {
                courseID = courseName.id;

            }
            
            var target = Path.Combine(_environment.ContentRootPath, "Course Material");
            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }

            foreach (var file in MaterialUploads)
            {
                if (file.Length <= 0) return;

                var filePath = Path.Combine(target, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                var newfile = new CourseMaterial
                {
                    name = file.FileName,
                    materialTypeID = materialType.id,
                    userId = user.id,
                    courseId = courseID,
                    fileStatus = 0,
                    materialPath = filePath,
                    fileSize = filePath.Length,
                    submission_date = DateTime.Now,
                };
                _contex.Materials?.Add(newfile);
                await _contex.SaveChangesAsync();
            }
        }
        public async Task<List<CourseMaterial>> GetAllForStudent(int id)// course id
        {

            var files = await _contex.Materials!.Where(f => f.courseId == id && f.fileStatus == CourseMaterial.FileStatus.Approved)
                .ToListAsync();
            return _mapper.Map<List<CourseMaterial>>(files);
        }
        public async Task<List<CourseMaterial>> GetAll(int id) //course id

        {

            var files = await _contex.Materials!
                .Where(f => f.courseId == id)
                .ToListAsync();
            return _mapper.Map<List<CourseMaterial>>(files);
        }
        public async Task UpdateFileAsync(string newName, int id)// new file name and material id
        {
            var target = Path.Combine(_environment.ContentRootPath, "Course Material");
            var material = await _contex.Materials!.FindAsync(id);
            if (material == null) { return; }
            //file change Name
            string fileType = Path.GetExtension(material.materialPath);
            string FileName = newName + fileType;
            var filePath = Path.Combine(target, FileName);
            System.IO.File.Move(material.materialPath, filePath);

            material.name = FileName;
            material.materialTypeID = material.materialTypeID;
            material.materialPath = material.materialPath;
            material.fileSize = material.fileSize;
            material.submission_date = material.submission_date;
            material.fileStatus = material.fileStatus;
            material.courseId = material.courseId;
            material.userId = material.userId;
            material.materialTopicId= material.materialTopicId;
            _contex.Materials.Update(material);
            await _contex.SaveChangesAsync();
        }
        public async Task DeleteFileAsync(int id)
        {
            var deleteFile = await _contex.Materials.FindAsync(id);
            if (deleteFile.fileStatus == CourseMaterial.FileStatus.Approved) { return; }
            if (deleteFile != null)
            {
                _contex.Materials.Remove(deleteFile);
                await _contex.SaveChangesAsync();
            }
        }//matedial id
        public  async Task FileApprove(string check , int id) //check = file status (Approved or reject) , id = material id
        {
            var material = await _contex.Materials!.FindAsync(id);
            if(material != null)
            {
                var status  = CourseMaterial.FileStatus.Pendding;
    

                if(check == "Approved")
                {
                    status = CourseMaterial.FileStatus.Approved;
                }
                if(check == "Reject")
                {
                    status = CourseMaterial.FileStatus.Reject;
                }
                material.name = material.name;
                material.materialTypeID= material.materialTypeID;
                material.materialPath = material.materialPath;
                material.fileSize = material.fileSize;
                material.submission_date = material.submission_date;
                material.fileStatus = status;
                material.courseId = material.courseId;
                _contex.Materials.Update(material);
                await _contex.SaveChangesAsync();
            }
        }

        public async Task AddToTopic(string topicName, int id)
        {
            var material = await _contex.Materials!.FindAsync(id);
            var topic = await _contex.Topics!.FirstOrDefaultAsync(t => t.name == topicName);
            if(topic ==null || material== null ||material.fileStatus == CourseMaterial.FileStatus.Reject || material.fileStatus == CourseMaterial.FileStatus.Pendding)
            {
                return;
            }

            material.name= material.name;
            material.materialTypeID= material.materialTypeID;
            material.materialPath = material.materialPath;  
            material.fileSize = material.fileSize;
            material.submission_date= material.submission_date;
            material.fileStatus= material.fileStatus;
            material.courseId = material.courseId;
            material.courseId=  material.courseId;
            material.materialTopicId = topic.id;
            _contex.Materials.Update(material);
            await _contex.SaveChangesAsync();
        }
    }
}