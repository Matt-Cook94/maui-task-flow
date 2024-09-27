using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace TaskFlowModels.Models
{
    public class TaskItem : INotifyPropertyChanged
    {
        public int TaskItemId { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CompletedDate { get; set; }
        public int ListItemId { get; set; }
        public ListItem ListItem { get; set; }
        public List<SubTaskItem> SubTasks { get; } = new();


        private bool _isStarred;
        public bool IsStarred
        {
            get => _isStarred;
            set
            {
                if (_isStarred != value)
                {
                    _isStarred = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isCompleted;
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                if (_isCompleted != value)
                {
                    _isCompleted = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
