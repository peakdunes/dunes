using DUNES.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DUNES.API.Repositories.Masters
{

    /// <summary>
    /// CRUD Master tables process
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MasterRepository<T> : IMasterRepository<T> where T : class
    {
        private readonly AppDbContext _context;


        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="context"></param>
        public MasterRepository(AppDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// get all information
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
        /// <summary>
        /// get all information for id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        /// <summary>
        /// add new record
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        /// <summary>
        /// update record by id
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<T> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        /// <summary>
        /// delete record by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteByIdAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null) return false;

            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        /// <summary>
        /// this utility searh in one table for a string field
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<IEnumerable<T>> SearchByFieldAsync(string fieldName, string value)
        {
            var parameter = Expression.Parameter(typeof(T), "x");

            // Access property dynamically
            var property = Expression.PropertyOrField(parameter, fieldName);

            if (property.Type != typeof(string))
                throw new InvalidOperationException($"Property {fieldName} is not a string.");

            // Build "x.fieldName.Contains(value)"
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
            var valueExpression = Expression.Constant(value, typeof(string));
            var containsCall = Expression.Call(property, containsMethod, valueExpression);

            var lambda = Expression.Lambda<Func<T, bool>>(containsCall, parameter);

            return await _context.Set<T>().Where(lambda).ToListAsync();
        }

    }
}
