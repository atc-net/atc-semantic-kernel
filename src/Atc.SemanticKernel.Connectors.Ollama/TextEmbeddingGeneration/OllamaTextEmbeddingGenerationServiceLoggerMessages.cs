namespace Atc.SemanticKernel.Connectors.Ollama.TextEmbeddingGeneration;

/// <summary>
/// OllamaTextEmbeddingGenerationService LoggerMessages.
/// </summary>
[SuppressMessage("Design", "MA0048:File name must match type name", Justification = "OK - By Design")]
public sealed partial class OllamaTextEmbeddingGenerationService
{
    [LoggerMessage(
        EventId = LoggingEventIdConstants.OllamaTextEmbeddingGenerationService.GenerateEmbeddingsStarted,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Starting to generate embeddings for '{input}'")]
    private partial void LogGenerateEmbeddingsStarted(
        string input,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.OllamaTextEmbeddingGenerationService.GenerateEmbeddingsSucceeded,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Successfully generated embeddings for '{input}'")]
    private partial void LogGenerateEmbeddingsSucceeded(
        string input,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.OllamaTextEmbeddingGenerationService.FailedToGenerateEmbeddings,
        Level = LogLevel.Error,
        Message = "{callerMethodName}({callerLineNumber}) - Failed to generate embeddings for '{input}'")]
    private partial void LogFailedToGenerateEmbeddings(
        string input,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);
}