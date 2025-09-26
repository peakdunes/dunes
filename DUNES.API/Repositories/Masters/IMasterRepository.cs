namespace DUNES.API.Repositories.Masters
{
    /// <summary>
    /// All CRUD master tables process
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMasterRepository<T> where T : class
    {
        /// <summary>
        /// Get all information for a master table
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync(CancellationToken ct);
        /// <summary>
        /// Get all information for a master table for id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<T> GetByIdAsync(int id, CancellationToken ct);
        /// <summary>
        /// Add new record
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<T> AddAsync(T entity, CancellationToken ct);
        /// <summary>
        /// Update master record by id
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<T> UpdateAsync(T entity, CancellationToken ct);
        /// <summary>
        /// delete master record by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> DeleteByIdAsync(int id, CancellationToken ct);

        /// <summary>
        /// Get record list for a table by string
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<T> SearchByFieldAsync(string fieldName, string value,CancellationToken ct);
    }
}
