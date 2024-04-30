using HttpClient client = new();
client.Timeout = TimeSpan.FromMinutes(5);
client.BaseAddress = new Uri("https://localhost:7252");

Console.WriteLine("Hint: type your question or type 'exit' to leave the conversation");
Console.WriteLine();

while (true)
{
    Console.Write("You: ");
    var input = Console.ReadLine();

    if (string.IsNullOrEmpty(input) ||
        input.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }

    await foreach (var msg in client.GetFromJsonAsAsyncEnumerable<StreamingChatMessageContent>(
                       $"/ollama/chat-completion/streaming?ask={input}"))
    {
        Console.Write(msg!.Content);
    }

    Console.WriteLine();
    Console.WriteLine("---");
}