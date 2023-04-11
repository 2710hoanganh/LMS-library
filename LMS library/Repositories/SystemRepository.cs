using AutoMapper;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using LMS_library.Data;
using Microsoft.AspNetCore.Http;

namespace LMS_library.Repositories
{
    public class SystemRepository : ISystemRepository
    {

        private readonly IMapper _mapper;
        private readonly DataDBContex _contex;
        private IWebHostEnvironment _webHostEnvironment;

        public SystemRepository(IMapper mapper, DataDBContex contex, IWebHostEnvironment webHostEnvironment)
        {
            _mapper = mapper;
            _contex = contex;
            _webHostEnvironment= webHostEnvironment;
        }
        public async Task<string> AddDetailAsync(SystemModel model)
        {
            var newDetail = _mapper.Map<SystemDetail>(model);
            _contex.System.Add(newDetail);
            await _contex.SaveChangesAsync();
            return ("create successfully .");
        }

        public async Task DeleteDetailAsync(int id)
        {
            var deleteDatail = await _contex.System!.FindAsync(id);
            if (deleteDatail != null)
            {
                _contex.System.Remove(deleteDatail);
                await _contex.SaveChangesAsync();
            }
        }

        public async Task<SystemModel> GetById(int id)
        {
            var detail = await _contex.System!.FindAsync(id);
            return _mapper.Map<SystemModel>(detail);
        }

        public async Task UpdateDetaiAsync(int id, SystemModel model)
        {
            if (id == model.id)
            {
                var detail = await _contex.System!.FindAsync(model.id);
                detail.schoolName = model.schoolName;
                detail.schoolWebSite = model.schoolWebSite;
                detail.shoolType = model.shoolType;
                detail.principal = model.principal;
                detail.libraryName = model.libraryName;
                detail.lybraryWebSite = model.lybraryWebSite;
                detail.lybraryPhone = model.lybraryPhone;
                detail.lybraryEmail = model.lybraryEmail;
                var updateDetail = _mapper.Map<SystemDetail>(detail);
                _contex.System?.Update(updateDetail);
                await _contex.SaveChangesAsync();
            }
        }



        public async  Task UploadImage(int id,IFormFile file)
        {
            var detail = await _contex.System!.FindAsync(id);
            if (detail == null ) { return; }
            var target = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\System\Avata");
            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }
            var filePath = Path.Combine(target, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            detail.schoolName = detail.schoolName;
            detail.schoolWebSite = detail.schoolWebSite;
            detail.shoolType = detail.shoolType;
            detail.principal = detail.principal;
            detail.libraryName = detail.libraryName;
            detail.lybraryWebSite = detail.lybraryWebSite;
            detail.lybraryPhone = detail.lybraryPhone;
            detail.lybraryEmail = detail.lybraryEmail;
            detail.image = filePath;
            var updateDetail = _mapper.Map<SystemDetail>(detail);
            _contex.System?.Update(updateDetail);
            await _contex.SaveChangesAsync();
        }

        public async Task ChangeImage(int id,IFormFile file)
        {
            var detail = await _contex.System!.FindAsync(id);
            if (detail == null) { return; }
            var target = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\System\Avata");
            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }
            var filePath = Path.Combine(target, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            System.IO.File.Delete(detail.image);
            detail.schoolName = detail.schoolName;
            detail.schoolWebSite = detail.schoolWebSite;
            detail.shoolType = detail.shoolType;
            detail.principal = detail.principal;
            detail.libraryName = detail.libraryName;
            detail.lybraryWebSite = detail.lybraryWebSite;
            detail.lybraryPhone = detail.lybraryPhone;
            detail.lybraryEmail = detail.lybraryEmail;
            detail.image = filePath;
            var updateDetail = _mapper.Map<SystemDetail>(detail);
            _contex.System?.Update(updateDetail);
            await _contex.SaveChangesAsync(); 
        }
        public async Task DeleteImageAsync(int id)
        {
            var detail = await _contex.System!.FindAsync(id);
            if (detail == null) { return; }
            System.IO.File.Delete(detail.image);
            detail.schoolName = detail.schoolName;
            detail.schoolWebSite = detail.schoolWebSite;
            detail.shoolType = detail.shoolType;
            detail.principal = detail.principal;
            detail.libraryName = detail.libraryName;
            detail.lybraryWebSite = detail.lybraryWebSite;
            detail.lybraryPhone = detail.lybraryPhone;
            detail.lybraryEmail = detail.lybraryEmail;
            detail.image = null;
            var updateDetail = _mapper.Map<SystemDetail>(detail);
            _contex.System?.Update(updateDetail);
            await _contex.SaveChangesAsync(); throw new NotImplementedException();
        }
    }
}
