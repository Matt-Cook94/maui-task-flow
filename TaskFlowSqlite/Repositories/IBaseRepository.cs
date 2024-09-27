namespace TaskFlowSqlite.Repositories
{
    public interface IBaseRepository<T> where T : new()
    {
        public Task<T> AddItemAsync(T item);
        public Task<T> GetItemAsync(int id);
        public Task<IEnumerable<T>> GetAllItemsAsync();
        public Task<T> UpdateItemAsync(T item);
        public Task<bool> DeleteItemAsync(T item);
    }
}
