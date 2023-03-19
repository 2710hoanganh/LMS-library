using AutoMapper;
using System.Security.Claims;
using System.IO;


namespace LMS_library.Repositories
{
    public class PrivateFileRepository : IPrivateFileRepository
    {

        private readonly DataDBContex _contex;
        private IHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;   

        public PrivateFileRepository(DataDBContex contex, IMapper mapper , IHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
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
            var target = Path.Combine(_environment.ContentRootPath, "Private File");
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
            var files = await _contex.PrivateFiles!.ToListAsync();
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
        }
        public async Task UpdateFileAsync(string newName, int id)
        {

                var target = Path.Combine(_environment.ContentRootPath, "Private File");
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

    }
}
