// ReSharper disable InconsistentNaming
namespace Atc.SemanticKernel.Connectors.Ollama;

[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "OK.")]
[SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "OK.")]
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "OK.")]
public abstract class OllamaServiceBase<T>
    where T : OllamaServiceBase<T>
{
    protected readonly ILogger<T> logger;

    protected readonly OllamaApiClient client;

    /// <summary>
    /// Storage for AI service attributes.
    /// </summary>
    internal Dictionary<string, object?> AiServiceAttributes { get; } = [];

    public IReadOnlyDictionary<string, object?> Attributes
        => AiServiceAttributes;

    /// <summary>
    /// Initializes a new instance of the OllamaServiceBase class using specified logger factory and API client.
    /// </summary>
    /// <param name="loggerFactory">The factory to create a logger, or null to use a null logger.</param>
    /// <param name="client">The Ollama API client used for communication.</param>
    /// <exception cref="ArgumentNullException">Thrown if the client is null.</exception>
    protected OllamaServiceBase(
        ILoggerFactory? loggerFactory,
        OllamaApiClient client)
    {
        ArgumentNullException.ThrowIfNull(client);

        logger = loggerFactory is not null
            ? loggerFactory.CreateLogger<T>()
            : NullLogger<T>.Instance;

        this.client = client;

        AddAttribute(AIServiceExtensions.ModelIdKey, client.SelectedModel);
    }

    /// <summary>
    /// Initializes a new instance of the OllamaServiceBase class using specified logger factory, endpoint URI, and model ID.
    /// </summary>
    /// <param name="loggerFactory">The factory to create a logger, or null to use a null logger.</param>
    /// <param name="endpoint">The URI of the endpoint to connect to, or null for the default endpoint.</param>
    /// <param name="modelId">The model ID to use, or null for the default model ID.</param>
    protected OllamaServiceBase(
        ILoggerFactory? loggerFactory,
        Uri? endpoint = null,
        string? modelId = null)
    {
        logger = loggerFactory is not null
            ? loggerFactory.CreateLogger<T>()
            : NullLogger<T>.Instance;

        var endpointUri = endpoint?.AbsoluteUri ?? OllamaConstants.DefaultEndpoint;

        this.client = new OllamaApiClient(
            endpointUri,
            modelId ?? OllamaConstants.DefaultModel);

        AddAttribute(AIServiceExtensions.ModelIdKey, client.SelectedModel);
        AddAttribute(AIServiceExtensions.EndpointKey, endpointUri);
    }

    /// <summary>
    /// Adds an attribute to the AI service attributes storage if the value is not null or empty.
    /// </summary>
    /// <param name="key">The key under which to store the value.</param>
    /// <param name="value">The value to store, if not null or empty.</param>
    internal void AddAttribute(
        string key,
        string? value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            AiServiceAttributes.Add(key, value);
        }
    }
}