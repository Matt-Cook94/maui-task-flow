using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskFlowModels.Models
{
    public class SubTaskItem
    {
        public int SubTaskItemId { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Completed { get; set; }
        public int TaskId { get; set; }
        public TaskItem TaskItem { get; set; }
    }
}
