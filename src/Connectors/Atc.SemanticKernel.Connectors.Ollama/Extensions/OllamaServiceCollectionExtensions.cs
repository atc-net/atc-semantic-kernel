// ReSharper disable ConvertToLocalFunction
namespace Atc.SemanticKernel.Connectors.Ollama.Extensions;

public static class OllamaServiceCollectionExtensions
{
    /// <summary>
    /// Adds the Ollama text generation service to the list.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance to augment.</param>
    /// <param name="endpoint">Endpoint</param>
    /// <param name="modelId">Model identifier</param>
    /// <returns>The same instance as <paramref name="services"/>.</returns>
    public static IServiceCollection AddOllamaTextGeneration(
        this IServiceCollection services,
        string? endpoint = null,
        string? modelId = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        Func<IServiceProvider, OllamaTextGenerationService> factory = (serviceProvider) =>
        {
            var client = new OllamaApiClient(
                endpoint ?? OllamaConstants.DefaultEndpoint,
                modelId ?? OllamaConstants.DefaultModel);

            return new OllamaTextGenerationService(client, serviceProvider.GetService<ILoggerFactory>());
        };

        services.AddSingleton<ITextGenerationService>(factory);

        return services;
    }

    /// <summary>
    /// Adds the Ollama text generation service to the list.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance to augment.</param>
    /// <param name="ollamaApiClient"><see cref="OllamaApiClient"/> to use for the service. If null, one must be available in the service provider when this service is resolved.</param>
    /// <returns>The same instance as <paramref name="services"/>.</returns>
    public static IServiceCollection AddOllamaTextGeneration(
        this IServiceCollection services,
        OllamaApiClient? ollamaApiClient = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        Func<IServiceProvider, OllamaTextGenerationService> factory = (serviceProvider)
            => new OllamaTextGenerationService(
                ollamaApiClient ?? serviceProvider.GetRequiredService<OllamaApiClient>(),
                serviceProvider.GetService<ILoggerFactory>());

        services.AddSingleton<ITextGenerationService>(factory);

        return services;
    }

    /// <summary>
    /// Adds the Ollama chat completion service to the list.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance to augment.</param>
    /// <param name="endpoint">Endpoint</param>
    /// <param name="modelId">Model identifier</param>
    /// <returns>The same instance as <paramref name="services"/>.</returns>
    public static IServiceCollection AddOllamaChatCompletion(
        this IServiceCollection services,
        string? endpoint = null,
        string? modelId = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        Func<IServiceProvider, OllamaChatCompletionService> factory = (serviceProvider) =>
        {
            var client = new OllamaApiClient(
                endpoint ?? OllamaConstants.DefaultEndpoint,
                modelId ?? OllamaConstants.DefaultModel);

            return new OllamaChatCompletionService(client, serviceProvider.GetService<ILoggerFactory>());
        };

        services.AddSingleton<IChatCompletionService>(factory);

        return services;
    }

    /// <summary>
    /// Adds the Ollama chat completion service to the list.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance to augment.</param>
    /// <param name="ollamaApiClient"><see cref="OllamaApiClient"/> to use for the service. If null, one must be available in the service provider when this service is resolved.</param>
    /// <returns>The same instance as <paramref name="services"/>.</returns>
    public static IServiceCollection AddOllamaChatCompletion(
        this IServiceCollection services,
        OllamaApiClient? ollamaApiClient = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        Func<IServiceProvider, OllamaChatCompletionService> factory = (serviceProvider)
            => new OllamaChatCompletionService(
                ollamaApiClient ?? serviceProvider.GetRequiredService<OllamaApiClient>(),
                serviceProvider.GetService<ILoggerFactory>());

        services.AddSingleton<IChatCompletionService>(factory);

        return services;
    }

    /// <summary>
    /// Adds Ollama text embedding generation service to the list.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance to augment.</param>
    /// <param name="endpoint">Ollama endpoint</param>
    /// <param name="modelId">Model identifier</param>
    /// <returns>The same instance as <paramref name="services"/>.</returns>
    public static IServiceCollection AddOllamaTextEmbeddingGeneration(
        this IServiceCollection services,
        string? endpoint = null,
        string? modelId = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        Func<IServiceProvider, OllamaTextEmbeddingGenerationService> factory = (serviceProvider) =>
        {
            var client = new OllamaApiClient(
                endpoint ?? OllamaConstants.DefaultEndpoint,
                modelId ?? OllamaConstants.DefaultModel);

            return new OllamaTextEmbeddingGenerationService(client, serviceProvider.GetService<ILoggerFactory>());
        };

#pragma warning disable SKEXP0001
        services.AddSingleton<IEmbeddingGenerationService<string, float>>(factory);
#pragma warning restore SKEXP0001

        return services;
    }

    /// <summary>
    /// Adds Ollama as the text embedding generation backend for semantic kernel
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance to augment.</param>
    /// <param name="ollamaApiClient"><see cref="OllamaApiClient"/> to use for the service. If null, one must be available in the service provider when this service is resolved.</param>
    /// <returns>The same instance as <paramref name="services"/>.</returns>
    public static IServiceCollection AddOllamaTextEmbeddingGeneration(
        this IServiceCollection services,
        OllamaApiClient? ollamaApiClient = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        Func<IServiceProvider, OllamaTextEmbeddingGenerationService> factory = (serviceProvider)
            => new OllamaTextEmbeddingGenerationService(
                ollamaApiClient ?? serviceProvider.GetRequiredService<OllamaApiClient>(),
                serviceProvider.GetService<ILoggerFactory>());

#pragma warning disable SKEXP0001
        services.AddSingleton<IEmbeddingGenerationService<string, float>>(factory);
#pragma warning restore SKEXP0001

        return services;
    }
}