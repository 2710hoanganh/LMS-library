using AutoMapper;

namespace LMS_library.Repositories
{
    public class SystemRepository : ISystemRepository
    {

        private readonly IMapper _mapper;
        private readonly DataDBContex _contex;

        public SystemRepository(IMapper mapper, DataDBContex contex)
        {
            _mapper = mapper;
            _contex = contex;
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
    }
}
