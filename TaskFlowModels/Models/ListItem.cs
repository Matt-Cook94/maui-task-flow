using System.Collections.ObjectModel;

namespace TaskFlowModels.Models
{
    public class ListItem
    {
        public int ListItemId { get; set; }
        public string? Name { get; set; }
        public bool IsDeletable { get; set; }
        public ObservableCollection<TaskItem> Tasks { get; } = new();
    }
}
