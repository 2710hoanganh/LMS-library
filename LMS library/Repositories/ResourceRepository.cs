using AutoMapper;
using LMS_library.Data;

namespace LMS_library.Repositories
{
    public class ResourceRepository : IResourceRepository
    {

        private readonly IMapper _mapper;
        private readonly DataDBContex _contex;

        public ResourceRepository(IMapper mapper, DataDBContex contex)
        {
            _mapper = mapper;
            _contex = contex;
        }

        public async Task<string> AddRessourceAsync(ResourceModel model)
        {
            var lesson = await _contex.Lessons.FirstOrDefaultAsync( l => l.name == model.lessonName );
            if ( lesson == null ) { return ("No lesson found"); }
            var resource = new ResourceList
            {
                lessonId = lesson.id
            };

            var newResource = _mapper.Map<ResourceList>(resource);
            _contex.ResourceLists.Add(newResource);
            await _contex.SaveChangesAsync();
            return ("create successfully .");
        }

        public async Task AddToTopic(string topicName, int id)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int id)
        {
            var material = await _contex.Materials.Where(m => m.resourceId == id).ToListAsync();
            var delete = await _contex.ResourceLists!.FindAsync(id);
            if (delete != null)
            {
                foreach(var m in material)
                {
                    m.resourceId = null;
                    await _contex.SaveChangesAsync();
                }

                _contex.ResourceLists.Remove(delete);
                await _contex.SaveChangesAsync();
            }
        }

        public async Task<List<ResourceList>> GetAll(int id)
        {
            var resources = await _contex.ResourceLists!.ToListAsync();
            return _mapper.Map<List<ResourceList>>(resources);
        }

        public async Task<List<ResourceList>> GetAllOnLesson(int id)
        {
            var resource = await _contex.ResourceLists!.Where(l => l.lessonId == id).ToListAsync();
            return _mapper.Map<List<ResourceList>>(resource);
        }

        public async Task Update(int id, ResourceModel model)
        {
            var lesson = await _contex.Lessons.FirstOrDefaultAsync(l => l.name == model.lessonName);
            var resource = await _contex.ResourceLists.FindAsync(id);
            if (resource != null|| lesson != null)
            {
                resource.lessonId = lesson.id;
                var update = _mapper.Map<ResourceList>(resource);
                _contex.ResourceLists?.Update(update);
                await _contex.SaveChangesAsync();
            }
        }
        public async Task AddToResource(string lessonName, int id)
        {
            var material = await _contex.Materials!.FindAsync(id);
            var lesson = await _contex.Lessons!.FirstOrDefaultAsync(t => t.name == lessonName);
            var resource = await _contex.ResourceLists.FirstOrDefaultAsync(r => r.lessonId == lesson.id);
            if (lesson == null || material == null || resource == null || material.fileStatus == CourseMaterial.FileStatus.Reject || material.fileStatus == CourseMaterial.FileStatus.Pendding)
            {
                return;
            }

            material.name = material.name;
            material.materialTypeID = material.materialTypeID;
            material.materialPath = material.materialPath;
            material.fileSize = material.fileSize;
            material.submission_date = material.submission_date;
            material.fileStatus = material.fileStatus;
            material.courseId = material.courseId;
            material.UserId = material.UserId;
            material.resourceId = resource.id;
            _contex.Materials.Update(material);
            await _contex.SaveChangesAsync();
        }
    }
}
