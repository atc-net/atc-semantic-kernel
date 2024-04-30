namespace Atc.SemanticKernel.Connectors.Ollama.TextGenerationService;

/// <summary>
/// OllamaTextGenerationService LoggerMessages.
/// </summary>
[SuppressMessage("Design", "MA0048:File name must match type name", Justification = "OK - By Design")]
public sealed partial class OllamaTextGenerationService
{
    [LoggerMessage(
        EventId = LoggingEventIdConstants.OllamaTextGenerationService.TextCompletionStarted,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Starting to generate text completion for '{prompt}'")]
    private partial void LogTextCompletionStarted(
        string prompt,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.OllamaTextGenerationService.TextCompletionSucceeded,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Successfully generated text completion for prompt '{prompt}'")]
    private partial void LogTextCompletionSucceeded(
        string prompt,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.OllamaTextGenerationService.TextCompletionStreamingStarted,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Starting to generate streaming text completion for '{prompt}'")]
    private partial void LogTextCompletionStreamingStarted(
        string prompt,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.OllamaTextGenerationService.TextCompletionStreamingSucceeded,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Successfully generated streaming text completion for prompt '{prompt}'")]
    private partial void LogTextCompletionStreamingSucceeded(
        string prompt,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);
}