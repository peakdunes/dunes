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
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync();
        /// <summary>
        /// Get all information for a master table for id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetByIdAsync(int id);
        /// <summary>
        /// Add new record
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> AddAsync(T entity);
        /// <summary>
        /// Update master record by id
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> UpdateAsync(T entity);
        /// <summary>
        /// delete master record by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteByIdAsync(int id);

       /// <summary>
       /// Get record list for a table by string 
       /// </summary>
       /// <param name="fieldName"></param>
       /// <param name="value"></param>
       /// <returns></returns>
        Task<IEnumerable<T>> SearchByFieldAsync(string fieldName, string value);
    }
}
