using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using TaskFlowSqlite.DataAccess;
using TaskFlowModels.Models;

namespace TaskFlowSqlite.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private static ConcurrentDictionary<string, TaskItem> _taskCache;

        private TaskFlowDbContext _dbContext;

        public TaskRepository(TaskFlowDbContext dbContext)
        {
            _dbContext = dbContext;

            if (_taskCache is null)
            {
                _taskCache = new ConcurrentDictionary<string, TaskItem>(_dbContext.Tasks.Include(t => t.SubTasks).ToDictionary(t => t.TaskItemId.ToString()));
            }
        }

        public async Task<TaskItem> AddItemAsync(TaskItem item)
        {
            EntityEntry<TaskItem> added = await _dbContext.Tasks.AddAsync(item);
            
            int affected = _dbContext.SaveChanges();
            
            if (affected == 1)
            {
                if (_taskCache is null) return null;
                
                return _taskCache.AddOrUpdate(item.TaskItemId.ToString(), item, UpdateCache);
            }
            return null;
        }

        public Task<TaskItem> GetItemAsync(int id)
        {
            if (_taskCache is null) return null;

            _taskCache.TryGetValue(id.ToString(), out TaskItem item);
            return Task.FromResult(item);
        }

        public Task<IEnumerable<TaskItem>> GetAllItemsAsync()
        {
            return Task.FromResult(_taskCache is null ? Enumerable.Empty<TaskItem>() : _taskCache.Values.OrderBy(t => t.TaskItemId));
        }

        public async Task<TaskItem> UpdateItemAsync(TaskItem item)
        {
            _dbContext.Tasks.Update(item);
            int affected = await _dbContext.SaveChangesAsync();   

            if (affected > 0)
            {
                return UpdateCache(item.TaskItemId.ToString(), item);
            }
            return null;
        }

        public async Task<bool> DeleteItemAsync(TaskItem item)
        {
            TaskItem t = _dbContext.Tasks.FirstOrDefault(i => i.TaskItemId == item.TaskItemId);

            if (t is null) return false;

            _dbContext.Tasks.Remove(item);
            int affected = await _dbContext.SaveChangesAsync();
            if (affected > 0)
            {
                if (_taskCache is null) return false;

                return _taskCache.Remove(item.TaskItemId.ToString(), out item);
            }
            else
            {
                return false;
            }
        }

        private TaskItem UpdateCache(string id, TaskItem item)
        {
            TaskItem old;
            if (_taskCache is not null)
            {
                if (_taskCache.TryGetValue(id, out old))
                {
                    if (_taskCache.TryUpdate(id, item, old))
                    {
                        return item;
                    }
                }
            }
            return null;
        }
    }
}
