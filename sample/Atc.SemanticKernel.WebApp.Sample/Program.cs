var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddKernel()
    .AddOllamaTextGeneration(
        builder.Configuration["Ollama:Endpoint"]!,
        builder.Configuration["Ollama:Model"]!)
    .AddOllamaChatCompletion(
        builder.Configuration["Ollama:Endpoint"]!,
        builder.Configuration["Ollama:Model"]!)
    .AddOllamaTextEmbeddingGeneration(
        builder.Configuration["Ollama:Endpoint"]!,
        builder.Configuration["Ollama:Model"]!);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/ollama/text-generation/non-streaming", async (string ask, Kernel kernel, CancellationToken cancellationToken) =>
    {
        var textGenerationService = kernel.GetRequiredService<ITextGenerationService>();
        var response = await textGenerationService.GetTextContentsAsync(ask, cancellationToken: cancellationToken);
        return response;
    })
    .WithName("OllamaTextGenerationNonStreaming")
    .WithOpenApi();

app.MapGet("/ollama/text-generation/streaming", IAsyncEnumerable<string> (string ask, Kernel kernel, CancellationToken cancellationToken) =>
    {
        return TextGenerationAsync();

        async IAsyncEnumerable<string> TextGenerationAsync()
        {
            var textGenerationService = kernel.GetRequiredService<ITextGenerationService>();
            var response = textGenerationService.GetStreamingTextContentsAsync(ask, cancellationToken: cancellationToken);

            await foreach (var item in response.WithCancellation(cancellationToken))
            {
                yield return item.Text ?? string.Empty;
            }
        }
    })
    .WithName("OllamaTextGenerationStreaming")
    .WithOpenApi();

app.MapGet("/ollama/chat-completion/non-streaming", async (string ask, Kernel kernel, CancellationToken cancellationToken) =>
    {
        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        var chatHistory = new ChatHistory();
        chatHistory.AddSystemMessage("You are a useful assistant that replies in a very short style");
        chatHistory.AddUserMessage(ask);

        var response = await chatCompletionService.GetChatMessageContentsAsync(chatHistory, cancellationToken: cancellationToken);
        return response[^1].Content;
    })
    .WithName("OllamaChatCompletionNonStreaming")
    .WithOpenApi();

app.MapGet("/ollama/chat-completion/streaming", IAsyncEnumerable<string> (string ask, Kernel kernel, CancellationToken cancellationToken) =>
    {
        return ChatCompletionAsync();

        async IAsyncEnumerable<string> ChatCompletionAsync()
        {
            var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

            var chatHistory = new ChatHistory();
            chatHistory.AddSystemMessage("You are a useful assistant that replies in a very short style");
            chatHistory.AddUserMessage(ask);

            var response = chatCompletionService.GetStreamingChatMessageContentsAsync(chatHistory, cancellationToken: cancellationToken);

            await foreach (var item in response.WithCancellation(cancellationToken))
            {
                yield return item.Content ?? string.Empty;
            }
        }
    })
    .WithName("OllamaChatCompletionStreaming")
    .WithOpenApi();

app.MapGet("/ollama/embedding-generation", async (string input, Kernel kernel, CancellationToken cancellationToken) =>
    {
        #pragma warning disable SKEXP0001
        var textEmbeddingGenerationService = kernel.GetRequiredService<ITextEmbeddingGenerationService>();
        #pragma warning restore SKEXP0001

        var embeddings = await textEmbeddingGenerationService.GenerateEmbeddingsAsync([input], cancellationToken: cancellationToken);
        return embeddings;
    })
    .WithName("OllamaEmbeddingGeneration")
    .WithOpenApi();

app.Run();