using DUNES.API.Utils.Responses;

namespace DUNES.API.Services.Masters
{
    public interface IMasterService<T> where T : class
    {
        Task<ApiResponse<IEnumerable<T>>> GetAllAsync();
        Task<ApiResponse<T>> GetByIdAsync(int id);
        Task<ApiResponse<T>> AddAsync(T entity);
        Task<ApiResponse<T>> UpdateAsync(T entity);
        Task<ApiResponse<bool>> DeleteByIdAsync(int id);
    }
}
