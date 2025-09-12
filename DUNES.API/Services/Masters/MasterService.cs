using DUNES.API.Repositories.Masters;
using DUNES.API.Utils.Responses;
using DUNES.Shared.Models;

namespace DUNES.API.Services.Masters
{
    /// <summary>
    /// Service for all master tables
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MasterService<T> : IMasterService<T> where T : class
    {
        private readonly IMasterRepository<T> _repository;


        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="repository"></param>
        public MasterService(IMasterRepository<T> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// get all data for this table
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<IEnumerable<T>>> GetAllAsync()
        {
            var result = await _repository.GetAllAsync();
            return ApiResponseFactory.Ok(result);
        }
        /// <summary>
        /// get one register for this id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<T>> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null
                ? ApiResponseFactory.NotFound<T>("No se encontró el recurso")
                : ApiResponseFactory.Ok(entity);
        }
        /// <summary>
        /// add record
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<ApiResponse<T>> AddAsync(T entity)
        {
            var created = await _repository.AddAsync(entity);
            return ApiResponseFactory.Created(created);
        }
        /// <summary>
        /// update a record
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<ApiResponse<T>> UpdateAsync(T entity)
        {
            var updated = await _repository.UpdateAsync(entity);
            return updated == null
                ? ApiResponseFactory.NotFound<T>("No se pudo actualizar (no encontrado)")
                : ApiResponseFactory.Ok(updated, "Actualizado correctamente");
        }
        /// <summary>
        /// delete a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<bool>> DeleteByIdAsync(int id)
        {
            var deleted = await _repository.DeleteByIdAsync(id);
            return deleted
                ? ApiResponseFactory.Ok(true, "Eliminado correctamente")
                : ApiResponseFactory.NotFound<bool>("No se encontró el recurso a eliminar");
        }
        /// <summary>
        /// Get all table records for a string field
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<T>>> SearchByFieldAsync(string fieldName, string value)
        {
            var results = await _repository.SearchByFieldAsync(fieldName, value);

            if (results == null || !results.Any())
                return ApiResponseFactory.NotFound<List<T>>($"No records found for {fieldName} containing '{value}'");

            return ApiResponseFactory.Ok(results.ToList(), "Search completed successfully");
        }
    }
}
