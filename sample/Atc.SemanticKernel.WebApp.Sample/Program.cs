var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddKernel()
    .AddOllamaTextGeneration(
        builder.Configuration["Ollama:Endpoint"]!,
        builder.Configuration["Ollama:Model"]!)
    .AddOllamaChatCompletion(
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

app.MapGet("/ollama/text-generation/streaming", (string ask, Kernel kernel, CancellationToken cancellationToken) =>
    {
        var textGenerationService = kernel.GetRequiredService<ITextGenerationService>();
        var response = textGenerationService.GetStreamingTextContentsAsync(ask, cancellationToken: cancellationToken);
        return response;
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

app.MapGet("/ollama/chat-completion/streaming", (string ask, Kernel kernel, CancellationToken cancellationToken) =>
    {
        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        var chatHistory = new ChatHistory();
        chatHistory.AddSystemMessage("You are a useful assistant that replies in a very short style");
        chatHistory.AddUserMessage(ask);

        var response = chatCompletionService.GetStreamingChatMessageContentsAsync(chatHistory, cancellationToken: cancellationToken);
        return response;
    })
    .WithName("OllamaChatCompletionStreaming")
    .WithOpenApi();

app.Run();