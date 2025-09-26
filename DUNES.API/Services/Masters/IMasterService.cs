using DUNES.API.Utils.Responses;
using DUNES.Shared.Models;

namespace DUNES.API.Services.Masters
{

    /// <summary>
    /// Service for all master tables
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    public interface IMasterService<T, TDto>
     where T : class
     where TDto : class
    {
        /// <summary>
        /// get all data for this table
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<IEnumerable<TDto>>> GetAllAsync(CancellationToken ct);
        /// <summary>
        /// get one register for this id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<TDto>> GetByIdAsync(int id, CancellationToken ct);
        /// <summary>
        /// add record
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<TDto>> AddAsync(TDto dto, CancellationToken ct );
        /// <summary>
        /// update a record
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<TDto>> UpdateAsync(TDto dto, CancellationToken ct);
        /// <summary>
        /// delete a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> DeleteByIdAsync(int id, CancellationToken ct);

        /// <summary>
        /// Get all table records for a string field
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<TDto>> SearchByFieldAsync(string fieldName, string value, CancellationToken ct);
    }
}
