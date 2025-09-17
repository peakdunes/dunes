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
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<IEnumerable<T>>> GetAllAsync(CancellationToken ct)
        {
            var result = await _repository.GetAllAsync(ct);
            return ApiResponseFactory.Ok(result);
        }
        /// <summary>
        /// get one register for this id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<T>> GetByIdAsync(int id, CancellationToken ct)
        {
            var entity = await _repository.GetByIdAsync(id, ct);
            return entity == null
                ? ApiResponseFactory.NotFound<T>("Information not found")
                : ApiResponseFactory.Ok(entity);
        }
        /// <summary>
        /// add record
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<T>> AddAsync(T entity, CancellationToken ct)
        {
            var created = await _repository.AddAsync(entity, ct);
            return ApiResponseFactory.Created(created);
        }
        /// <summary>
        /// update a record
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<T>> UpdateAsync(T entity, CancellationToken ct)
        {
            var updated = await _repository.UpdateAsync(entity, ct);
            return updated == null
                ? ApiResponseFactory.NotFound<T>("Record not updated")
                : ApiResponseFactory.Ok(updated, "Update successfull");
        }
        /// <summary>
        ///  delete a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<bool>> DeleteByIdAsync(int id, CancellationToken ct)
        {
            var deleted = await _repository.DeleteByIdAsync(id, ct);
            return deleted
                ? ApiResponseFactory.Ok(true, "Record deleted")
                : ApiResponseFactory.NotFound<bool>("Record not found");
        }
        /// <summary>
        /// Get all table records for a string field
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<T>>> SearchByFieldAsync(string fieldName, string value, CancellationToken ct)
        {
            var results = await _repository.SearchByFieldAsync(fieldName, value, ct);

            if (results == null || !results.Any())
                return ApiResponseFactory.NotFound<List<T>>($"No records found for {fieldName} containing '{value}'");

            return ApiResponseFactory.Ok(results.ToList(), "Search completed successfully");
        }
    }
}
