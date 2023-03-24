using AutoMapper;

namespace LMS_library.Repositories
{
    public class MaterialTypeRepository : IMaterialTypeRepository
    {
        public readonly IMapper _mapper;
        public readonly DataDBContex _contex;
        public MaterialTypeRepository(IMapper mapper ,DataDBContex contex)
        {
            _contex= contex;
            _mapper = mapper;
        }


        public async Task<string> AddMaterialTypeAsync(MaterialTypeModel model)
        {
            var newType = _mapper.Map<MaterialType>(model);
            _contex.MaterialTypes.Add(newType);
            await _contex.SaveChangesAsync();
            return ("create successfully .");
        }

        public async Task DeleteMaterialTypeAsync(int id)
        {
            var delete = await _contex.MaterialTypes!.FindAsync(id);
            if (delete != null)
            {
                _contex.MaterialTypes.Remove(delete);
                await _contex.SaveChangesAsync();
            }
        }//type id

        public async Task<List<MaterialType>> GetAll()
        {
            var all = await _contex.MaterialTypes!.ToListAsync();
            return _mapper.Map<List<MaterialType>>(all);
        }

        public async Task<MaterialTypeModel> GetById(int id) //type id
        {
            var type = await _contex.MaterialTypes!.FindAsync(id);
            return _mapper.Map<MaterialTypeModel>(type);
        }

        public async Task UpdateMaterialTypeAsync(int id, MaterialTypeModel model)//type id
        {
            if (id == model.id)
            {
                var type = await _contex.MaterialTypes!.FindAsync(model.id);
                type.name = model.name;
                var updatepType = _mapper.Map<MaterialType>(type);
                _contex.MaterialTypes?.Update(updatepType);
                await _contex.SaveChangesAsync();
            }
        }
    }
}

