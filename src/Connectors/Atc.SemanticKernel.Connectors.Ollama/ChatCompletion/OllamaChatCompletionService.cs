namespace Atc.SemanticKernel.Connectors.Ollama.ChatCompletion;

/// <summary>
/// Ollama chat completion service.
/// </summary>
[SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "OK.")]
public sealed partial class OllamaChatCompletionService
    : OllamaServiceBase<OllamaChatCompletionService>, IChatCompletionService
{
    // TODO: Add partial source-generated logging

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaChatCompletionService"/> class.
    /// </summary>
    /// <param name="client"><see cref="OllamaApiClient"/>.</param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> to use for logging. If null, no logging will be performed.</param>
    public OllamaChatCompletionService(
        OllamaApiClient client,
        ILoggerFactory? loggerFactory = null)
        : base(
            loggerFactory,
            client)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaChatCompletionService"/> class.
    /// </summary>
    /// <param name="endpoint">Custom Message API compatible endpoint</param>
    /// <param name="modelId">Model name</param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> to use for logging. If null, no logging will be performed.</param>
    public OllamaChatCompletionService(
        Uri endpoint,
        string modelId,
        ILoggerFactory? loggerFactory = null)
        : base(
            loggerFactory,
            endpoint,
            modelId)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaChatCompletionService"/> class.
    /// </summary>
    /// <param name="modelId">Model name</param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> to use for logging. If null, no logging will be performed.</param>
    public OllamaChatCompletionService(
        string modelId,
        ILoggerFactory? loggerFactory = null)
        : base(
            loggerFactory,
            endpoint: null,
            modelId)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaChatCompletionService"/> class.
    /// </summary>
    /// <param name="endpoint">Custom Message API compatible endpoint</param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> to use for logging. If null, no logging will be performed.</param>
    public OllamaChatCompletionService(
        Uri endpoint,
        ILoggerFactory? loggerFactory = null)
        : base(
            loggerFactory,
            endpoint,
            modelId: null)
    {
    }

    public async Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(
        ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null,
        CancellationToken cancellationToken = default)
    {
        var chat = new Chat(client, _ => { });

        foreach (var message in chatHistory)
        {
            if (message.Role == AuthorRole.System)
            {
                await chat.SendAs(ChatRole.System, message.Content, cancellationToken);
            }
        }

        var lastUserMessage = chatHistory.LastOrDefault(x => x.Role == AuthorRole.User);
        var messageToSend = lastUserMessage?.Content ?? string.Empty;

        var history = await chat.Send(messageToSend, cancellationToken);

        chatHistory.AddAssistantMessage(history.Last().Content);

        return chatHistory;
    }

    public async IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(
        ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var messages = new List<StreamingChatMessageContent>();
        var responseStreamer = new ActionResponseStreamer<ChatResponseStream>(x =>
        {
            messages.Add(new StreamingChatMessageContent(AuthorRole.Assistant, x.Message.Content, modelId: client.SelectedModel));
        });

        var chat = client.Chat(responseStreamer);

        foreach (var message in chatHistory)
        {
            if (message.Role == AuthorRole.System)
            {
                await chat.SendAs(ChatRole.System, message.Content, cancellationToken);
            }
        }

        var lastUserMessage = chatHistory.LastOrDefault(x => x.Role == AuthorRole.User);
        var messageToSend = lastUserMessage?.Content ?? string.Empty;

        await chat.Send(messageToSend, cancellationToken);

        foreach (var message in messages)
        {
            yield return new StreamingChatMessageContent(AuthorRole.Assistant, message.Content, modelId: message.ModelId);
        }
    }
}