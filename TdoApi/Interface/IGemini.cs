using TdoApi.Models;

namespace TdoApi.Interface
{
    public interface IGemini
    {
        Task<AICommand> UnderstandCommand(string message,IEnumerable<TodoTask> todos);
    }
}
