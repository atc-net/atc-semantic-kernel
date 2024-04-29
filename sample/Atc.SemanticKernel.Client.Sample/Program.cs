using HttpClient client = new();
client.BaseAddress = new Uri("https://localhost:7252");

while (true)
{
    Console.WriteLine("Ask:");

    var q = Console.ReadLine();

    await foreach (var msg in client.GetFromJsonAsAsyncEnumerable<string>(
                       $"/ollama/chat-completion/streaming?ask={q}"))
    {
        Console.Write(msg);
    }

    Console.WriteLine();
}