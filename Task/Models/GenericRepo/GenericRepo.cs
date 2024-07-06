using Microsoft.EntityFrameworkCore;
using Task.Models.Database;

namespace Task.Models.GenericRepo
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        private readonly AppDBContext db;
        private readonly ILogger<GenericRepo<T>> logger;

        public GenericRepo(AppDBContext db, ILogger<GenericRepo<T>> logger)
        {
            this.db = db;
            this.logger = logger;
        }

        public async Task<T> AddAsync(T obj)
        {
            try
            {
                var entityEntry = await db.Set<T>().AddAsync(obj);
                await db.SaveChangesAsync();
                return entityEntry.Entity;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding entity");
                throw;
            }
        }

        public async Task<T> DeleteAsync(T obj)
        {
            try
            {
                var entityEntry = db.Set<T>().Remove(obj);
                await db.SaveChangesAsync();
                return entityEntry.Entity;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting entity");
                throw;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await db.Set<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving all entities");
                throw;
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            try
            {
                return await db.Set<T>().FindAsync(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error retrieving entity by id: {id}");
                throw;
            }
        }

        public async Task<T> UpdateAsync(T obj)
        {
            try
            {
                var updatedObj = db.Set<T>().Update(obj);
                await db.SaveChangesAsync();
                return updatedObj.Entity;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating entity");
                throw;
            }
        }
    }
}
