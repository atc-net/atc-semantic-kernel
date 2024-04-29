namespace Atc.SemanticKernel.Connectors.Ollama.ChatCompletion;

/// <summary>
/// OllamaChatCompletionService LoggerMessages.
/// </summary>
[SuppressMessage("Design", "MA0048:File name must match type name", Justification = "OK - By Design")]
public sealed partial class OllamaChatCompletionService
{
    [LoggerMessage(
        EventId = LoggingEventIdConstants.OllamaChatCompletionService.XXX,
        Level = LogLevel.Error,
        Message = "{callerMethodName}({callerLineNumber}) - XXX '{xxx}': {errorMessage}")]
    private partial void LogXXXX(
        string xxx,
        string? errorMessage,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);
}