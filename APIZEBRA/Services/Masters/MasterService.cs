using APIZEBRA.Repositories.Masters;
using APIZEBRA.Utils.Responses;

namespace APIZEBRA.Services.Masters
{
    public class MasterService<T> : IMasterService<T> where T : class
    {
        private readonly IMasterRepository<T> _repository;

        public MasterService(IMasterRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<IEnumerable<T>>> GetAllAsync()
        {
            var result = await _repository.GetAllAsync();
            return ApiResponseFactory.Ok(result);
        }

        public async Task<ApiResponse<T>> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null
                ? ApiResponseFactory.NotFound<T>("No se encontró el recurso")
                : ApiResponseFactory.Ok(entity);
        }

        public async Task<ApiResponse<T>> AddAsync(T entity)
        {
            var created = await _repository.AddAsync(entity);
            return ApiResponseFactory.Created(created);
        }

        public async Task<ApiResponse<T>> UpdateAsync(T entity)
        {
            var updated = await _repository.UpdateAsync(entity);
            return updated == null
                ? ApiResponseFactory.NotFound<T>("No se pudo actualizar (no encontrado)")
                : ApiResponseFactory.Ok(updated, "Actualizado correctamente");
        }

        public async Task<ApiResponse<bool>> DeleteByIdAsync(int id)
        {
            var deleted = await _repository.DeleteByIdAsync(id);
            return deleted
                ? ApiResponseFactory.Ok(true, "Eliminado correctamente")
                : ApiResponseFactory.NotFound<bool>("No se encontró el recurso a eliminar");
        }
    }
}
