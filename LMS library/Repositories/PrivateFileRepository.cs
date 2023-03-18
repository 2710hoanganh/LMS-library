using AutoMapper;
using System.Security.Claims;


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




        public async Task DownloadFileById(int id)
        {
            throw new NotImplementedException();
        }

 
    }
}
