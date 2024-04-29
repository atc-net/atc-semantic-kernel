namespace Atc.SemanticKernel.Connectors.Ollama.EmbeddingGeneration;

#pragma warning disable SKEXP0001
public sealed class OllamaTextEmbeddingGenerationService
    : OllamaServiceBase<OllamaTextEmbeddingGenerationService>, ITextEmbeddingGenerationService
#pragma warning restore SKEXP0001
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaTextEmbeddingGenerationService"/> class.
    /// </summary>
    /// <param name="client"><see cref="OllamaApiClient"/>.</param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> to use for logging. If null, no logging will be performed.</param>
    public OllamaTextEmbeddingGenerationService(
        OllamaApiClient client,
        ILoggerFactory? loggerFactory = null)
        : base(
            loggerFactory,
            client)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaTextEmbeddingGenerationService"/> class.
    /// </summary>
    /// <param name="endpoint">Custom Message API compatible endpoint</param>
    /// <param name="modelId">Model name</param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> to use for logging. If null, no logging will be performed.</param>
    public OllamaTextEmbeddingGenerationService(
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
    /// Initializes a new instance of the <see cref="OllamaTextEmbeddingGenerationService"/> class.
    /// </summary>
    /// <param name="modelId">Model name</param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> to use for logging. If null, no logging will be performed.</param>
    public OllamaTextEmbeddingGenerationService(
        string modelId,
        ILoggerFactory? loggerFactory = null)
        : base(
            loggerFactory,
            endpoint: null,
            modelId)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaTextEmbeddingGenerationService"/> class.
    /// </summary>
    /// <param name="endpoint">Custom Message API compatible endpoint</param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> to use for logging. If null, no logging will be performed.</param>
    public OllamaTextEmbeddingGenerationService(
        Uri endpoint,
        ILoggerFactory? loggerFactory = null)
        : base(
            loggerFactory,
            endpoint,
            modelId: null)
    {
    }

    public async Task<IList<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(
        IList<string> data,
        Kernel? kernel = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(data);

        var result = new List<ReadOnlyMemory<float>>(data.Count);

        foreach (var text in data)
        {
            var response = await client.GenerateEmbeddings(text, cancellationToken);
            if (response is not null)
            {
                var floatArray = Array.ConvertAll(response.Embedding, item => (float)item);
                var embedding = new ReadOnlyMemory<float>(floatArray);
                result.Add(embedding);
            }
            else
            {
                // TODO: Log error
                ////logger.LogError("Unable to connect to ollama at {url} with model {model}", Attributes["base_url"], Attributes["model_id"]);
            }
        }

        return result;
    }
}