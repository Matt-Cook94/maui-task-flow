using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TaskFlowSqlite.Repositories;
using TaskFlow.Services;
using TaskFlowModels.Models;
using TaskFlow.Helpers;

namespace TaskFlow.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<ListItem> listItems = new();

        [ObservableProperty]
        ObservableCollection<TaskItem> tasks = new();

        [ObservableProperty]
        ObservableCollection<SubTaskItem> subTasks = new();

        [ObservableProperty]
        ObservableCollection<TaskItem> completedTasks = new();

        [ObservableProperty]
        string? taskText;

        [ObservableProperty]
        string? subTaskText;

        [ObservableProperty]
        string? listItemText;

        [ObservableProperty]
        ListItem? currentListItem;

        [ObservableProperty]
        TaskItem? selectedTaskItem;

        [ObservableProperty]
        bool showCompletedTaskList;

        [ObservableProperty]
        bool taskSelected;

        List<TaskItem> _allTasks = new();

        ITaskRepository _taskRepo;
        IListRepository _listRepo;
        IDialogService _dialogService;

        public MainViewModel(ITaskRepository taskRepo, IListRepository listRepo, IDialogService dialogService)
        {
            _taskRepo = taskRepo;
            _listRepo = listRepo;
            _dialogService = dialogService;

            LoadDataAsync();
        }

        private async void LoadDataAsync()
        {
            var lists = await _listRepo.GetAllItemsAsync();

            ListItems = new ObservableCollection<ListItem>(lists);

            CurrentListItem = ListItems.First();

            await FillTaskItems();
        }

        private async Task FillTaskItems()
        {
            var tasks = await _taskRepo.GetAllItemsAsync();
            
            _allTasks = new List<TaskItem>(tasks);
            Tasks = new ObservableCollection<TaskItem>(_allTasks.Where(t => !t.IsCompleted));

            var completedTasks = _allTasks.Where(t => t.IsCompleted);
            CompletedTasks = new ObservableCollection<TaskItem>(completedTasks);
        }

        [RelayCommand]
        private async Task AddTask()
        {
            if (string.IsNullOrWhiteSpace(TaskText))
                return;

            TaskItem newTask = new TaskItem()
            {
                Description = TaskText,
                CreatedDate = DateTime.UtcNow,
                ListItemId = CurrentListItem.ListItemId
            };

            if (CurrentListItem.ListItemId == (int)ListItemType.Starred)
            {
                newTask.IsStarred = true;
            }

            var itemAdded = await _taskRepo.AddItemAsync(newTask);

            if (itemAdded is not null)
            {
                _allTasks.Add(newTask);
                Tasks.Add(newTask);
                TaskText = string.Empty;
            }
            else
            {
                await _dialogService.ShowConfirmationAsync("Error", "Failed to delete item.");
            }
        }

        [RelayCommand]
        private async Task DeleteTask(TaskItem task)
        {
            var deleted = await _taskRepo.DeleteItemAsync(task);

            if (deleted)
            {
                Tasks.Remove(task);
            }
            else
            {
                await _dialogService.ShowConfirmationAsync("Error", "Failed to delete item.");
            }
            
            TaskSelected = false;
        }

        [RelayCommand]
        private void SelectTaskItem(TaskItem task)
        {
            SelectedTaskItem = task;
            TaskSelected = true;

            SubTasks = new ObservableCollection<SubTaskItem>(task.SubTasks);
        }

        [RelayCommand]
        private async Task ToggleCompleteTask(TaskItem task)
        {
            task.CompletedDate = DateTime.UtcNow;

            var updatedTask = await _taskRepo.UpdateItemAsync(task);

            if (updatedTask is not null)
            {
                if (task.IsCompleted)
                {
                    Tasks.Remove(task);
                    CompletedTasks.Add(task);
                    if (SelectedTaskItem.Equals(task))
                    {
                        TaskSelected = false;
                    }
                }
                else
                {
                    CompletedTasks.Remove(task);
                    Tasks.Add(task);
                }
            }
            else
            {
                await _dialogService.ShowConfirmationAsync("Error", "Task completion failed.");
            }
        }

        [RelayCommand]
        private async Task ToggleStar(TaskItem task)
        {
            task.IsStarred = !task.IsStarred;

            await _taskRepo.UpdateItemAsync(task);
        }

        [RelayCommand]
        private async Task AddListItem()
        {
            var listName = await _dialogService.DisplayPromptAsync("Create List", "Add new name for list.");

            if (string.IsNullOrWhiteSpace(listName))
                return;

            ListItem newListItem= new ListItem() { Name = listName, IsDeletable = true };

            var itemAdded = await _listRepo.AddItemAsync(newListItem);

            if (itemAdded is not null)
            {
                ListItems.Add(newListItem);

                // open newly added list
                SelectListItem(newListItem);
            }
            else
            {
                await _dialogService.ShowConfirmationAsync("Error", "Failed to create new list.");
            }
        }

        [RelayCommand]
        private async Task DeleteListItem(ListItem listItem)
        {
            if (ListItems.Contains(listItem) && listItem.IsDeletable)
            {
                await _listRepo.DeleteItemAsync(listItem);
                ListItems.Remove(listItem);
            }
        }

        [RelayCommand]
        private void SelectListItem(ListItem listItem)
        {
            CurrentListItem = listItem;
            List<TaskItem> taskItems;

            if (listItem.ListItemId == (int)ListItemType.AllTasks)
            {
                taskItems = _allTasks;
            }
            else if(listItem.ListItemId == (int)ListItemType.Starred)
            {
                taskItems = _allTasks.Where(t => t.IsStarred).ToList();
            }
            else
            {
                taskItems = _allTasks.Where(t => t.TaskItemId == listItem.ListItemId).ToList();
            }

            var uncompletedTasks = taskItems.Where(t => !t.IsCompleted);
            var completedTasks = taskItems.Where(t => t.IsCompleted);

            TaskSelected = false;

            Tasks = new ObservableCollection<TaskItem>(uncompletedTasks);
            CompletedTasks = new ObservableCollection<TaskItem>(completedTasks);
        }

        [RelayCommand]
        private void ShowCompletedTasks()
        {
            ShowCompletedTaskList = !ShowCompletedTaskList;
        }

        [RelayCommand]
        private async Task AddSubTask()
        {
            if (string.IsNullOrWhiteSpace(SubTaskText) || SelectedTaskItem is null)
                return;

            var subTask = new SubTaskItem
            {
                Description = SubTaskText,
                CreatedDate = DateTime.Now,
                TaskId = SelectedTaskItem.TaskItemId
            };

            SelectedTaskItem.SubTasks.Add(subTask);

            var itemAdded = await _taskRepo.UpdateItemAsync(SelectedTaskItem);

            if (itemAdded is not null)
            {
                SubTasks.Add(subTask);
                SubTaskText = string.Empty;
            }
            else
            {
                await _dialogService.ShowConfirmationAsync("Error", "Failed to Add sub task.");
            }
        }

        [RelayCommand]
        private async Task CompleteSubTask(SubTaskItem subTask)
        {
            var task = await _taskRepo.GetItemAsync(subTask.TaskId);

            await _taskRepo.UpdateItemAsync(task);
        }
    }
}
