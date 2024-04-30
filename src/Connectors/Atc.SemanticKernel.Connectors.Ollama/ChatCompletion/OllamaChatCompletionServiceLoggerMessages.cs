namespace Atc.SemanticKernel.Connectors.Ollama.ChatCompletion;

/// <summary>
/// OllamaChatCompletionService LoggerMessages.
/// </summary>
[SuppressMessage("Design", "MA0048:File name must match type name", Justification = "OK - By Design")]
public sealed partial class OllamaChatCompletionService
{
    [LoggerMessage(
        EventId = LoggingEventIdConstants.OllamaChatCompletionService.ChatMessageContentStarted,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Starting to generate chat completion for '{prompt}'")]
    private partial void LogChatCompletionStarted(
        string prompt,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.OllamaChatCompletionService.ChatMessageContentSucceeded,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Successfully generated chat completion for prompt '{prompt}'")]
    private partial void LogChatCompletionSucceeded(
        string prompt,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.OllamaChatCompletionService.ChatMessageContentStreamingStarted,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Starting to generate streaming chat completion for '{prompt}'")]
    private partial void LogChatCompletionStreamingStarted(
        string prompt,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.OllamaChatCompletionService.ChatMessageContentStreamingSucceeded,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Successfully generated streaming chat completion for prompt '{prompt}'")]
    private partial void LogChatCompletionStreamingSucceeded(
        string prompt,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);
}