using System.Collections.ObjectModel;
using TaskFlowModels.Models;

namespace TaskFlowSqlite.Repositories
{
    public interface ITaskRepository : IBaseRepository<TaskItem>;
}
