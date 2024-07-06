namespace Task.Models.GenericRepo
{
    public interface IGenericRepo<T> where T : class
    {
        public Task<T> AddAsync(T obj);
        public Task<T> UpdateAsync(T obj);
        public Task<T> DeleteAsync(T obj);
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<T> GetByIdAsync(int id);

    }
}
