namespace Atc.SemanticKernel.Connectors.Ollama;

internal static class LoggingEventIdConstants
{
    internal static class OllamaChatCompletionService
    {
        public const int ChatMessageContentStarted = 10_000;
        public const int ChatMessageContentSucceeded = 10_010;
        public const int ChatMessageContentStreamingStarted = 10_020;
        public const int ChatMessageContentStreamingSucceeded = 10_030;
    }

    internal static class OllamaTextGenerationService
    {
        public const int TextCompletionStarted = 11_000;
        public const int TextCompletionSucceeded = 11_010;
        public const int TextCompletionStreamingStarted = 11_020;
        public const int TextCompletionStreamingSucceeded = 11_030;
    }

    internal static class OllamaTextEmbeddingGenerationService
    {
        public const int GenerateEmbeddingsStarted = 12_000;
        public const int GenerateEmbeddingsSucceeded = 12_010;
        public const int FailedToGenerateEmbeddings = 12_020;
    }
}