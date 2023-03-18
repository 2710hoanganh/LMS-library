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
        public async Task UpdateFileAsync(int id, PrivateFileModel model)
        {
            if (id == model.id)
            {

            }
        }

    }
}
