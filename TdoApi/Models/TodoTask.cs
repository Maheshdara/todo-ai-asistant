using System.ComponentModel.DataAnnotations;

namespace TdoApi.Models
{
    public class TodoTask
    {
        [Key]
        public int TodoId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
