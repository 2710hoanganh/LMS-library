using AutoMapper;

using System.IO;
using static System.Net.WebRequestMethods;


namespace LMS_library.Repositories
{
    public class PrivateFileRepository : IPrivateFileRepository
    {
        private readonly IMapper _mapper;
        private readonly DataDBContex _contex;
        private IHostEnvironment _environment;

        public PrivateFileRepository(DataDBContex contex, IMapper mapper , IHostEnvironment environment)
        {
            _contex = contex;
            _mapper = mapper;
            _environment = environment;
        }
        public async Task PostFileAsync( PrivateFileUpload files)
        {
            var target = Path.Combine(_environment.ContentRootPath,"Private File");
            Directory.CreateDirectory(target);
            if(files !=null ) 
            {
                var filePath = Path.Combine(target, files.formFile.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await files.formFile.CopyToAsync(stream);
                }
            }
        
        }

        public async Task DownloadFileById(int id)
        {
            throw new NotImplementedException();
        }


    }
}
