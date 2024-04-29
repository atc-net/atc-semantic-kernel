namespace Atc.SemanticKernel.Connectors.Ollama.TextGenerationService;

public sealed class OllamaTextGenerationService
    : OllamaServiceBase<OllamaTextGenerationService>, ITextGenerationService
{
    // TODO: Add partial source-generated logging

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaTextGenerationService"/> class.
    /// </summary>
    /// <param name="modelId">Model name</param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> to use for logging. If null, no logging will be performed.</param>
    public OllamaTextGenerationService(
        string modelId,
        ILoggerFactory? loggerFactory = null)
        : base(loggerFactory, endpoint: null, modelId)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaTextGenerationService"/> class.
    /// </summary>
    /// <param name="endpoint">Custom Message API compatible endpoint</param>
    /// <param name="modelId">Model name</param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> to use for logging. If null, no logging will be performed.</param>
    public OllamaTextGenerationService(
        Uri endpoint,
        string modelId,
        ILoggerFactory? loggerFactory = null)
        : base(loggerFactory, endpoint, modelId)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaTextGenerationService"/> class.
    /// </summary>
    /// <param name="endpoint">Custom Message API compatible endpoint</param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> to use for logging. If null, no logging will be performed.</param>
    public OllamaTextGenerationService(
        Uri endpoint,
        ILoggerFactory? loggerFactory = null)
        : base(loggerFactory, endpoint, modelId: null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaTextGenerationService"/> class.
    /// </summary>
    /// <param name="client"><see cref="OllamaApiClient"/>.</param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> to use for logging. If null, no logging will be performed.</param>
    public OllamaTextGenerationService(
        OllamaApiClient client,
        ILoggerFactory? loggerFactory = null)
        : base(loggerFactory, client)
    {
    }

    public async Task<IReadOnlyList<TextContent>> GetTextContentsAsync(
        string prompt,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null,
        CancellationToken cancellationToken = default)
    {
        var response = await client.GetCompletion(
            prompt,
            context: null,
            CancellationToken.None);

        var textContext = new TextContent(response.Response);
        return [textContext];
    }

    public async IAsyncEnumerable<StreamingTextContent> GetStreamingTextContentsAsync(
        string prompt,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var messages = new List<string>();
        var responseStreamer = new ActionResponseStreamer<GenerateCompletionResponseStream>(x =>
        {
            messages.Add(x.Response);
        });

        var generateCompletionRequest = new GenerateCompletionRequest
        {
            Prompt = prompt,
            Model = client.SelectedModel,
            Stream = true,
            Context = [],
        };

        await client.StreamCompletion(generateCompletionRequest, responseStreamer, cancellationToken);

        foreach (var message in messages)
        {
            yield return new StreamingTextContent(message);
        }
    }
}