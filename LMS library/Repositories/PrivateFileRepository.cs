using AutoMapper;
using System.Security.Claims;
using System.IO;
using LMS_library.Data;
using LMS_library.Models;

namespace LMS_library.Repositories
{
    public class PrivateFileRepository : IPrivateFileRepository
    {

        private readonly DataDBContex _contex;
        private IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;   

        public PrivateFileRepository(DataDBContex contex, IMapper mapper , IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _contex = contex;
            _mapper = mapper;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task PostMultiFileAsync(List<IFormFile> privateFileUploads)
        {
            var result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            if (result == null)
            {
                return;
            }
            var user = await _contex.Users.FirstOrDefaultAsync(u => u.email == result);

            var target = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\System\Private File");
            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }

            foreach(var file in privateFileUploads)
            {
                if (file.Length <= 0) return;

                var filePath = Path.Combine(target, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                var newfile = new PrivateFiles
                {
                    fileName = file.FileName,
                    fileType = Path.GetExtension(filePath),
                    fileSize = filePath.Length.ToString(),
                    filePath = filePath,
                    uploadAt = DateTime.Now,
                    updateAt = DateTime.Now,
                    userId = user.id,
                };
                _contex.PrivateFiles?.Add(newfile);
                await _contex.SaveChangesAsync();
            }
        }
        public async Task<List<PrivateFiles>> GetAll()
        {
            var result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var files = await _contex.PrivateFiles!.Where(f => f.userId == Int32.Parse(result)).ToListAsync();
            return _mapper.Map<List<PrivateFiles>>(files);
        }
        public async Task DeleteFileAsync(int id)
        {
            var deleteFile = await _contex.PrivateFiles.FindAsync(id);
            if (deleteFile != null)
            {
                _contex.PrivateFiles.Remove(deleteFile);
                await _contex.SaveChangesAsync();
            }
        }//file id
        public async Task UpdateFileAsync(string newName, int id) //newName = new file name + file id
        {


            var target = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\System\Private File");
            var file = await _contex.PrivateFiles!.FindAsync(id);
                if (file == null) { return; }
            //file change Name
                string FileName = newName + file.fileType;
                var filePath = Path.Combine(target, FileName);
                System.IO.File.Move(file.filePath, filePath);

                file.fileName = FileName;
                file.fileType = file.fileType;
                file.fileSize = file.fileSize;
                file.filePath = filePath;
                file.uploadAt = file.uploadAt;
                file.updateAt = DateTime.Now;
                file.userId = file.userId;
                _contex.PrivateFiles.Update(file);
                await _contex.SaveChangesAsync();

        }

        public async Task<List<PrivateFileModel>> Filter(string? type)
        {
            var file = _contex.PrivateFiles.AsQueryable();
            if (!string.IsNullOrEmpty(type))
            {
                file = _contex.PrivateFiles.OrderBy(m => m.fileType.Contains(type) );
            }
            

            var result = file.Select(m => new PrivateFileModel
            {
                id= m.id,
                fileName = m.fileName,
                fileType= m.fileType,
                fileSize= m.fileSize,
                uploadAt = m.uploadAt,
                updateAt= m.updateAt,
            });
            return result.ToList();
        }
        public async Task<List<PrivateFileModel>> Search(string? search)
        {
            var file = _contex.PrivateFiles.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                file = _contex.PrivateFiles.Where(m => m.fileName.Contains(search)|| m.fileType.Contains(search));
            }


            var result = file.Select(m => new PrivateFileModel
            {
                id = m.id,
                fileName = m.fileName,
                fileType = m.fileType,
                fileSize = m.fileSize,
                uploadAt = m.uploadAt,
                updateAt = m.updateAt,
            });
            return result.ToList();
        }
    }
}
