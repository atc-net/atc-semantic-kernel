# Introduction

This repository contains various connectors and plugins for integrating Large Language Models (LLMs) via Semantic Kernel.

# Table of Content

- [Introduction](#introduction)
- [Table of Content](#table-of-content)
- [SemanticKernel Connectors](#semantickernel-connectors)
  - [Atc.SemanticKernel.Connectors.Ollama](#atcsemantickernelconnectorsollama)
    - [Wire-Up Using KernelBuilder/ServiceCollection Extensions](#wire-up-using-kernelbuilderservicecollection-extensions)
      - [Setup with KernelBuilder](#setup-with-kernelbuilder)
      - [Setup with ServiceCollection](#setup-with-servicecollection)
    - [Examples](#examples)
      - [Using the OllamaChatCompletionService](#using-the-ollamachatcompletionservice)
      - [Using the OllamaTextGenerationService](#using-the-ollamatextgenerationservice)
      - [Using the OllamaTextEmbeddingGenerationService](#using-the-ollamatextembeddinggenerationservice)
- [Requirements](#requirements)
- [How to contribute](#how-to-contribute)

# SemanticKernel Connectors

## Atc.SemanticKernel.Connectors.Ollama

[![NuGet Version](https://img.shields.io/nuget/v/atc.semantickernel.connectors.ollama.svg?logo=nuget&style=for-the-badge)](https://www.nuget.org/packages/atc.semantickernel.connectors.ollama)

The `Atc.SemanticKernel.Connectors.Ollama` package contains a connector for integrating with [Ollama](https://ollama.com/) .

It supports the following capabilities by implementing the interfaces (`IChatCompletionService`, `ITextGenerationService`, `ITextEmbeddingGenerationService`)

> Note: Embedding generation is marked as experimental in Semantic Kernel

### Wire-Up Using KernelBuilder/ServiceCollection Extensions

To seamlessly integrate Ollama services into your application, you can utilize the provided [`KernelBuilder`](src/Connectors/Atc.SemanticKernel.Connectors.Ollama/Extensions/OllamaKernelBuilderExtensions.cs) and [`ServiceCollection`](src/Connectors/Atc.SemanticKernel.Connectors.Ollama/Extensions/OllamaServiceCollectionExtensions.cs) extension methods. These methods simplify the setup process and ensure that the Ollama services are correctly configured and ready to use within your application's service architecture.

Both methods ensure that the Ollama services are added to the application's service collection and configured according to the specified parameters, making them available throughout your application via dependency injection.

The configuration examples below utilizes the application's settings (typically defined in appsettings.json) to configure each Ollama service with appropriate endpoints and model identifiers.

#### Setup with KernelBuilder

The `KernelBuilder` extensions allow for fluent configuration and registration of services. Here’s how you can wire up Ollama services using `KernelBuilder`:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add Kernel services
builder.Services.AddKernel()
    .AddOllamaTextGeneration(
        builder.Configuration["Ollama:Endpoint"],
        builder.Configuration["Ollama:Model"])
    .AddOllamaChatCompletion(
        builder.Configuration["Ollama:Endpoint"],
        builder.Configuration["Ollama:Model"])
    .AddOllamaTextEmbeddingGeneration(
        builder.Configuration["Ollama:Endpoint"],
        builder.Configuration["Ollama:Model"]);
```

#### Setup with ServiceCollection

Alternatively, if you're configuring services directly through `IServiceCollection`, here's how you can add Ollama services:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Configure Ollama services directly
builder.Services
    .AddOllamaTextGeneration(
        builder.Configuration["Ollama:Endpoint"],
        builder.Configuration["Ollama:Model"])
    .AddOllamaChatCompletion(
        builder.Configuration["Ollama:Endpoint"],
        builder.Configuration["Ollama:Model"])
    .AddOllamaTextEmbeddingGeneration(
        builder.Configuration["Ollama:Endpoint"],
        builder.Configuration["Ollama:Model"]);
```

### Examples

#### Using the OllamaChatCompletionService

To utilize the `OllamaChatCompletionService` for chat completions, you can initialize it with an `OllamaApiClient` or a custom API endpoint, and a model ID. Below is an example of how to use the service to handle chat sessions.

```csharp
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
var history = new ChatHistory();

// Add needed messages to current chat history
history.AddSystemMessage("...");
history.AddUserMessage(input);

var chatResponse = await chat.GetChatMessageContentsAsync(history);
Console.WriteLine(chatResponse[^1].Content);
```

#### Using the OllamaTextGenerationService

The `OllamaTextGenerationService` offers text generation capabilities using a specified model. This service can be initialized using an `OllamaApiClient` or a custom API endpoint, and a model ID. Below is an example of how to use the service to handle text generation.

```csharp
var textGen = kernel.GetRequiredService<ITextGenerationService>();
var response = await textGen.GetTextContentsAsync("The weather in January in Denmark is usually ");
Console.WriteLine(response[^1].Text);
```

#### Using the OllamaTextEmbeddingGenerationService

The OllamaTextEmbeddingGenerationService provides functionality to generate text embeddings. This service can be initiated with an `OllamaApiClient` or a custom endpoint, and model ID. Below is an example of how to use the service to handle text generation.

```csharp
#pragma warning disable SKEXP0001
var embeddingGenerationService = kernel.GetRequiredService<ITextEmbeddingGenerationService>();
#pragma warning restore SKEXP0001

List<string> texts = ["Hello"];

var embeddings = await embeddingGenerationService.GenerateEmbeddingsAsync(texts);
Console.WriteLine($"Embeddings Length: {embeddings.Count}");
```

# Requirements

* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
* [Ollama](https://ollama.com/) with one or more downloaded models

# How to contribute

[Contribution Guidelines](https://atc-net.github.io/introduction/about-atc#how-to-contribute)

[Coding Guidelines](https://atc-net.github.io/introduction/about-atc#coding-guidelines)
