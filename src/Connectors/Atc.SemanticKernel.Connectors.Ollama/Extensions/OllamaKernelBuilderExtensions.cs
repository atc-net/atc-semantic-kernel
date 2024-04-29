// ReSharper disable ConvertToLocalFunction
namespace Atc.SemanticKernel.Connectors.Ollama.Extensions;

public static class OllamaKernelBuilderExtensions
{
    /// <summary>
    /// Adds the Ollama text generation service to the list.
    /// </summary>
    /// <param name="builder">The <see cref="IKernelBuilder"/> instance to augment.</param>
    /// <param name="endpoint">Ollama endpoint</param>
    /// <param name="modelId">Model identifier</param>
    /// <returns>The same instance as <paramref name="builder"/>.</returns>
    public static IKernelBuilder AddOllamaTextGeneration(
        this IKernelBuilder builder,
        string? endpoint = null,
        string? modelId = null)
    {
        ArgumentNullException.ThrowIfNull(builder);

        Func<IServiceProvider, OllamaTextGenerationService> factory = (serviceProvider) =>
        {
            var client = new OllamaApiClient(
                endpoint ?? OllamaConstants.DefaultEndpoint,
                modelId ?? OllamaConstants.DefaultModel);

            return new OllamaTextGenerationService(client, serviceProvider.GetService<ILoggerFactory>());
        };

        builder.Services.AddSingleton<ITextGenerationService>(factory);

        return builder;
    }

    /// <summary>
    /// Adds the Ollama text generation service to the list.
    /// </summary>
    /// <param name="builder">The <see cref="IKernelBuilder"/> instance to augment.</param>
    /// <param name="ollamaApiClient"><see cref="OllamaApiClient"/> to use for the service. If null, one must be available in the service provider when this service is resolved.</param>
    /// <returns>The same instance as <paramref name="builder"/>.</returns>
    public static IKernelBuilder AddOllamaTextGeneration(
        this IKernelBuilder builder,
        OllamaApiClient? ollamaApiClient = null)
    {
        ArgumentNullException.ThrowIfNull(builder);

        Func<IServiceProvider, OllamaTextGenerationService> factory = (serviceProvider)
            => new OllamaTextGenerationService(
                ollamaApiClient ?? serviceProvider.GetRequiredService<OllamaApiClient>(),
                serviceProvider.GetService<ILoggerFactory>());

        builder.Services.AddSingleton<ITextGenerationService>(factory);

        return builder;
    }

    /// <summary>
    /// Adds the Ollama chat completion service to the list.
    /// </summary>
    /// <param name="builder">The <see cref="IKernelBuilder"/> instance to augment.</param>
    /// <param name="endpoint">Ollama endpoint</param>
    /// <param name="modelId">Model identifier</param>
    /// <returns>The same instance as <paramref name="builder"/>.</returns>
    public static IKernelBuilder AddOllamaChatCompletion(
        this IKernelBuilder builder,
        string? endpoint = null,
        string? modelId = null)
    {
        ArgumentNullException.ThrowIfNull(builder);

        Func<IServiceProvider, OllamaChatCompletionService> factory = (serviceProvider) =>
        {
            var client = new OllamaApiClient(
                endpoint ?? OllamaConstants.DefaultEndpoint,
                modelId ?? OllamaConstants.DefaultModel);

            return new OllamaChatCompletionService(client, serviceProvider.GetService<ILoggerFactory>());
        };

        builder.Services.AddSingleton<IChatCompletionService>(factory);

        return builder;
    }

    /// <summary>
    /// Adds the Ollama chat completion service to the list.
    /// </summary>
    /// <param name="builder">The <see cref="IKernelBuilder"/> instance to augment.</param>
    /// <param name="ollamaApiClient"><see cref="OllamaApiClient"/> to use for the service. If null, one must be available in the service provider when this service is resolved.</param>
    /// <returns>The same instance as <paramref name="builder"/>.</returns>
    public static IKernelBuilder AddOllamaChatCompletion(
        this IKernelBuilder builder,
        OllamaApiClient? ollamaApiClient = null)
    {
        ArgumentNullException.ThrowIfNull(builder);

        Func<IServiceProvider, OllamaChatCompletionService> factory = (serviceProvider)
            => new OllamaChatCompletionService(
                ollamaApiClient ?? serviceProvider.GetRequiredService<OllamaApiClient>(),
                serviceProvider.GetService<ILoggerFactory>());

        builder.Services.AddSingleton<IChatCompletionService>(factory);

        return builder;
    }

    /// <summary>
    /// Adds Ollama text embedding generation service to the list.
    /// </summary>
    /// <param name="builder">The <see cref="IKernelBuilder"/> instance to augment.</param>
    /// <param name="endpoint">Ollama endpoint</param>
    /// <param name="modelId">Model identifier</param>
    /// <returns>The same instance as <paramref name="builder"/>.</returns>
    public static IKernelBuilder AddOllamaTextEmbeddingGeneration(
        this IKernelBuilder builder,
        string? endpoint = null,
        string? modelId = null)
    {
        ArgumentNullException.ThrowIfNull(builder);

        Func<IServiceProvider, OllamaTextEmbeddingGenerationService> factory = (serviceProvider) =>
        {
            var client = new OllamaApiClient(
                endpoint ?? OllamaConstants.DefaultEndpoint,
                modelId ?? OllamaConstants.DefaultModel);

            return new OllamaTextEmbeddingGenerationService(client, serviceProvider.GetService<ILoggerFactory>());
        };

#pragma warning disable SKEXP0001
        builder.Services.AddSingleton<IEmbeddingGenerationService<string, float>>(factory);
#pragma warning restore SKEXP0001

        return builder;
    }

    /// <summary>
    /// Adds Ollama as the text embedding generation backend for semantic kernel
    /// </summary>
    /// <param name="builder">The <see cref="IKernelBuilder"/> instance to augment.</param>
    /// <param name="ollamaApiClient"><see cref="OllamaApiClient"/> to use for the service. If null, one must be available in the service provider when this service is resolved.</param>
    /// <returns>The same instance as <paramref name="builder"/>.</returns>
    public static IKernelBuilder AddOllamaTextEmbeddingGeneration(
        this IKernelBuilder builder,
        OllamaApiClient? ollamaApiClient = null)
    {
        ArgumentNullException.ThrowIfNull(builder);

        Func<IServiceProvider, OllamaTextEmbeddingGenerationService> factory = (serviceProvider)
            => new OllamaTextEmbeddingGenerationService(
                ollamaApiClient ?? serviceProvider.GetRequiredService<OllamaApiClient>(),
                serviceProvider.GetService<ILoggerFactory>());

#pragma warning disable SKEXP0001
        builder.Services.AddSingleton<IEmbeddingGenerationService<string, float>>(factory);
#pragma warning restore SKEXP0001
        return builder;
    }
}