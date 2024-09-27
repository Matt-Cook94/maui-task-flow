using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using TaskFlowModels.Models;
using TaskFlowSqlite.DataAccess;

namespace TaskFlowSqlite.Repositories
{
    public class ListRepository : IListRepository
    {
        private static ConcurrentDictionary<string, ListItem> _listCache;

        private TaskFlowDbContext _dbContext;

        public ListRepository(TaskFlowDbContext dbContext)
        {
            _dbContext = dbContext;

            if (_listCache is null)
            {
                _listCache = new ConcurrentDictionary<string, ListItem>(_dbContext.Lists.ToDictionary(t => t.ListItemId.ToString()));
            }
        }

        public async Task<ListItem> AddItemAsync(ListItem item)
        {
            var result = await _dbContext.AddAsync(item);
            _dbContext.SaveChanges();

            EntityEntry<ListItem> added = await _dbContext.Lists.AddAsync(item);

            int affected = _dbContext.SaveChanges();

            if (affected == 1)
            {
                if (_listCache is null) return null;

                return _listCache.AddOrUpdate(item.ListItemId.ToString(), item, UpdateCache);
            }
            return null;
        }

        public Task<ListItem> GetItemAsync(int id)
        {
            if (_listCache is null) return null;

            _listCache.TryGetValue(id.ToString(), out ListItem item);
            return Task.FromResult(item);
        }

        public Task<IEnumerable<ListItem>> GetAllItemsAsync()
        {
            return Task.FromResult(_listCache is null ? Enumerable.Empty<ListItem>() : _listCache.Values.OrderBy(l => l.ListItemId));
        }

        public async Task<ListItem> UpdateItemAsync(ListItem item)
        {
            _dbContext.Lists.Update(item);
            int affected = await _dbContext.SaveChangesAsync();

            if (affected == 1)
            {
                return UpdateCache(item.ListItemId.ToString(), item);
            }
            return null;
        }

        public async Task<bool> DeleteItemAsync(ListItem item)
        {
            ListItem t = _dbContext.Lists.FirstOrDefault(i => item.ListItemId == item.ListItemId);

            if (t is null) return false;

            _dbContext.Lists.Remove(item);
            int affected = await _dbContext.SaveChangesAsync();
            if (affected == 1)
            {
                if (_listCache is null) return false;

                return _listCache.Remove(item.ListItemId.ToString(), out item);
            }
            else
            {
                return false;
            }
        }

        private ListItem UpdateCache(string id, ListItem item)
        {
            ListItem old;
            if (_listCache is not null)
            {
                if (_listCache.TryGetValue(id, out old))
                {
                    if (_listCache.TryUpdate(id, item, old))
                    {
                        return item;
                    }
                }
            }
            return null;
        }
    }
}
