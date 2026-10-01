        using System.Text;
        using System.Text.Json;
        using TdoApi.Interface;
        using TdoApi.Models;

        namespace TdoApi.Services
        {
        public class GeminiService : IGemini
        {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public GeminiService(
        IConfiguration configuration, 
        HttpClient httpClient)
        {
        _configuration = configuration;
        _httpClient = httpClient;
        }

        public async Task<AICommand> UnderstandCommand(
        string message,
        IEnumerable<TodoTask> todos)
        {
        var apiKey = _configuration["Gemini:ApiKey"];
        var model = _configuration["Gemini:Model"];

        var url =
        $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent";

        var todoJson = JsonSerializer.Serialize(todos);

        var prompt = $@"
        You are an intelligent Todo Assistant.

        The user can create, read, update and delete Todo tasks.

        Existing Todo data:

        {todoJson}

        User request:

        {message}

        Understand the user's request and return ONLY valid JSON.

        Supported actions:

        create
        get
        update
        delete
        unknown

        JSON format:

        {{
        ""action"": ""create"",
        ""todoId"": null,
        ""title"": ""Learn React"",
        ""newTitle"": null,
        ""description"": ""Prepare React interview"",
        ""newDescription"": null,
        ""isCompleted"": false,
        ""dueDate"": ""2026-09-26"",
        ""createdDate"": null,
        ""searchText"": null,
        ""dateFilter"": null
        }}

        Rules:

        1. For creating a Todo use action = ""create"".

        2. For retrieving Todos use action = ""get"".

        3. For updating a Todo use action = ""update"".

        4. For deleting a Todo use action = ""delete"".

        5. If the user says completed, use isCompleted = true.

        6. If the user says pending, use isCompleted = false.

        7. If the user asks for a specific title or description,
        use searchText.

        8. If the user asks for today's todos,
        use dateFilter = ""today"".

        9. If the user asks for tomorrow's todos,
        use dateFilter = ""tomorrow"".

        10. If the user asks for overdue todos,
        use dateFilter = ""overdue"".

        11. If the user asks for a Todo by ID,
        use todoId.

        12. Never invent a TodoId.

        13. If multiple Todos could match the request,
        return action = ""unknown"".

        14. Return ONLY valid JSON.
        ";
        var requestBody = new
        {
        contents = new[]
        {
            new
            {
                parts = new[]
                {
                    new
                    {
                        text = prompt
                    }
                }
            }
        }
        };

        var json = JsonSerializer.Serialize(requestBody);

        using var request = new HttpRequestMessage(
        HttpMethod.Post,
        url);

        request.Headers.Add(
        "x-goog-api-key",
        apiKey);

        request.Content = new StringContent(
        json,
        Encoding.UTF8,
        "application/json");



        var response =
        await _httpClient.SendAsync(request);

        var responseContent =
        await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
        throw new Exception(responseContent);
        }

        using var document =
        JsonDocument.Parse(responseContent);

        var text =
        document.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        var cleanJson = text!
        .Replace("```json", "")
        .Replace("```", "")
        .Trim();

        var command =
        JsonSerializer.Deserialize<AICommand>(
            cleanJson,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return command ?? new AICommand
        {
        Action = "unknown"
        };
        }
        }
        }