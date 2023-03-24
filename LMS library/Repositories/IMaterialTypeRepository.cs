namespace LMS_library.Repositories
{
    public interface IMaterialTypeRepository
    {
        public Task<List<MaterialType>> GetAll();
        public Task<MaterialTypeModel> GetById(int id);
        public Task<string> AddMaterialTypeAsync(MaterialTypeModel model);
        public Task UpdateMaterialTypeAsync(int id, MaterialTypeModel model);
        public Task DeleteMaterialTypeAsync(int id);
    }
}
