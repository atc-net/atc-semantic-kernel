namespace Atc.SemanticKernel.Connectors.Ollama.ChatCompletion;

/// <summary>
/// Ollama chat completion service.
/// </summary>
[SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "OK.")]
public sealed partial class OllamaChatCompletionService
    : OllamaServiceBase<OllamaChatCompletionService>, IChatCompletionService
{
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
        var lastUserMessage = chatHistory.LastOrDefault(x => x.Role == AuthorRole.User);
        var messageToSend = lastUserMessage?.Content ?? string.Empty;

        LogChatCompletionStarted(messageToSend);

        var chat = new Chat(client, _ => { });

        foreach (var message in chatHistory)
        {
            if (message.Role == AuthorRole.System)
            {
                await chat.SendAs(ChatRole.System, message.Content, cancellationToken);
            }
        }

        var history = await chat.Send(messageToSend, cancellationToken);

        chatHistory.AddAssistantMessage(history.Last().Content);

        var responseMessage = chatHistory[^1].Content ?? string.Empty;

        LogChatCompletionSucceeded(messageToSend);

        return
        [
            new ChatMessageContent(AuthorRole.Assistant, responseMessage),
        ];
    }

    public async IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(
        ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var lastUserMessage = chatHistory.LastOrDefault(x => x.Role == AuthorRole.User);
        var messageToSend = lastUserMessage?.Content ?? string.Empty;

        LogChatCompletionStreamingStarted(messageToSend);

        var messages = new List<StreamingChatMessageContent>();

        var responseQueue = new Queue<bool>();

        var responseStreamer = new ActionResponseStreamer<ChatResponseStream>(x =>
        {
            if (responseQueue.Count <= 0 ||
                !responseQueue.Dequeue())
            {
                return;
            }

            messages.Add(new StreamingChatMessageContent(AuthorRole.Assistant, x.Message.Content, modelId: client.SelectedModel));
            responseQueue.Enqueue(item: true);
        });

        var chat = client.Chat(responseStreamer);

        // Send system instructions to the LLM
        foreach (var message in chatHistory)
        {
            if (message.Role == AuthorRole.System)
            {
                await chat.SendAs(ChatRole.System, message.Content, cancellationToken);
            }
        }

        // Some LLMs models return responses to system instruction, so we only want to stream messages back from this point
        responseQueue.Enqueue(item: true);
        await chat.Send(messageToSend, cancellationToken);

        foreach (var message in messages)
        {
            yield return new StreamingChatMessageContent(AuthorRole.Assistant, message.Content, modelId: message.ModelId);
        }

        LogChatCompletionStreamingSucceeded(messageToSend);
    }
}