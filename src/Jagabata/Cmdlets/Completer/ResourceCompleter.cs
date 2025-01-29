using System.Collections;
using System.Management.Automation;
using System.Management.Automation.Language;
using Jagabata.Resources;

namespace Jagabata.Cmdlets.Completer;

internal enum ResourceCompleteType
{
    Resource,
    Id
}

internal class ResourceCompletionsAttribute(ResourceCompleteType completeType, params ResourceType[] types)
    : ArgumentCompleterAttribute, IArgumentCompleterFactory
{
    public ResourceCompletionsAttribute(params ResourceType[] types) : this(ResourceCompleteType.Resource, types)
    {
    }

    public ResourceType[] ResourceTypes { get; init; } = types;
    public ResourceCompleteType CompleteType { get; init; } = completeType;

    public IArgumentCompleter Create()
    {
        return CompleteType switch
        {
            ResourceCompleteType.Resource => new ResourceCompleter(ResourceTypes),
            ResourceCompleteType.Id => new ResourceIdCompleter(ResourceTypes),
            _ => throw new NotImplementedException()
        };
    }
}

internal class ResourceCompleter(ResourceType[] types) : IArgumentCompleter
{
    public ResourceType[] ResourceTypes { get; init; } = types;
    public IEnumerable<CompletionResult> CompleteArgument(string commandName,
                                                          string parameterName,
                                                          string wordToComplete,
                                                          CommandAst commandAst,
                                                          IDictionary fakeBoundParameters)
    {
        foreach (var item in Caches.GetEnumerator(ResourceTypes))
        {
            var name = $"{item.Type}:{item.Id}";
            var tooltip = $"[{name}] {item.Description}";
            if (name.StartsWith(wordToComplete, StringComparison.OrdinalIgnoreCase))
            {
                yield return new CompletionResult(name, name, CompletionResultType.ParameterValue, tooltip);
            }
        }
    }
}

internal class ResourceIdCompleter(ResourceType[] types) : IArgumentCompleter
{
    public ResourceType[] ResourceTypes { get; init; } = types;
    public IEnumerable<CompletionResult> CompleteArgument(string commandName,
                                                          string parameterName,
                                                          string wordToComplete,
                                                          CommandAst commandAst,
                                                          IDictionary fakeBoundParameters)
    {
        foreach (var item in Caches.GetEnumerator(ResourceTypes))
        {
            var id = $"{item.Id}";
            var name = $"{item.Type}:{id}";
            var tooltip = $"[{name}] {item.Description}";
            if (id.StartsWith(wordToComplete, StringComparison.OrdinalIgnoreCase))
            {
                yield return new CompletionResult(id, name, CompletionResultType.ParameterValue, tooltip);
            }
            else if (name.StartsWith(wordToComplete, StringComparison.OrdinalIgnoreCase))
            {
                yield return new CompletionResult(id, name, CompletionResultType.ParameterValue, tooltip);
            }
        }
    }
}
