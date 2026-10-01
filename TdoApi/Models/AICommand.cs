namespace TdoApi.Models
{
    public class AICommand
    {
        public string Action { get; set; } = "unknown";

        public int? TodoId { get; set; }

        public string? Title { get; set; }

        public string? NewTitle { get; set; }

        public string? Description { get; set; }

        public string? NewDescription { get; set; }

        public bool? IsCompleted { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? SearchText { get; set; }

        public string? DateFilter { get; set; }

    }
}
