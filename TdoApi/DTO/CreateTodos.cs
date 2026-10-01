namespace TdoApi.DTO
{
    public class CreateTodos
    {

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }


        public DateTime? DueDate { get; set; }

    }
}
