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

    // TODO: Documentation
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