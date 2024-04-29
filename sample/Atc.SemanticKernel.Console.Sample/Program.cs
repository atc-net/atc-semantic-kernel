var ollamaChat = new OllamaChatCompletionService(new Uri("http://localhost:11434"), "phi3");
var ollamaText = new OllamaTextGenerationService(new Uri("http://localhost:11434"), "phi3");
var ollamaEmbedding = new OllamaTextEmbeddingGenerationService(new Uri("http://localhost:11434"), "phi3");

// semantic kernel builder
var builder = Kernel.CreateBuilder();
builder.Services.AddKeyedSingleton<IChatCompletionService>("ollamaChat", ollamaChat);
builder.Services.AddKeyedSingleton<ITextGenerationService>("ollamaText", ollamaText);
#pragma warning disable SKEXP0001
builder.Services.AddKeyedSingleton<ITextEmbeddingGenerationService>("ollamaEmbedding", ollamaEmbedding);
#pragma warning restore SKEXP0001

var kernel = builder.Build();

Console.WriteLine("====================");
Console.WriteLine("TEXT GENERATION DEMO");
Console.WriteLine("====================");
var textGen = kernel.GetRequiredService<ITextGenerationService>();
var response = await textGen.GetTextContentsAsync("The weather in January in Toronto is usually ");
Console.WriteLine(response[^1].Text);

Console.WriteLine("====================");
Console.WriteLine("CHAT COMPLETION DEMO");
Console.WriteLine("====================");
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
var history = new ChatHistory();
history.AddSystemMessage("You are a useful assistant that replies with short messages.");
Console.WriteLine("Hint: type your question or type 'exit' to leave the conversation");

// Chat loop
while (true)
{
    Console.Write("You: ");
    var input = Console.ReadLine();

    if (string.IsNullOrEmpty(input) ||
        input.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }

    history.AddUserMessage(input);
    history = (ChatHistory)await chatCompletionService.GetChatMessageContentsAsync(history);
    Console.WriteLine(history[^1].Content);
    Console.WriteLine("---");
}

Console.WriteLine("====================");
Console.WriteLine("EMBEDDING DEMO");
Console.WriteLine("====================");

#pragma warning disable SKEXP0001
var embeddingGenerationService = kernel.GetRequiredService<ITextEmbeddingGenerationService>();
#pragma warning restore SKEXP0001

List<string> texts = ["Hello"];

var embeddings = await embeddingGenerationService.GenerateEmbeddingsAsync(texts);
Console.WriteLine($"Embeddings Length: {embeddings.Count}");

Console.ReadLine();