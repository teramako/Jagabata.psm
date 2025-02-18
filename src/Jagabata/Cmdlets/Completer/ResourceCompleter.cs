using System.Collections;
using System.Management.Automation;
using System.Management.Automation.Language;

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
    public string? FilterKey { get; init; }
    public string[]? FilterValues { get; init; }

    public IArgumentCompleter Create()
    {
        return CompleteType switch
        {
            ResourceCompleteType.Resource => new ResourceCompleter(ResourceTypes),
            ResourceCompleteType.Id => FilterKey is null || FilterValues is null
                ? new ResourceIdCompleter(ResourceTypes)
                : new ResourceIdCompleter(ResourceTypes, FilterKey, FilterValues),
            _ => throw new NotImplementedException()
        };
    }
}

internal class ResourceCompleter(ResourceType[] types) : ResourceCompleterBase
{
    public ResourceType[] ResourceTypes { get; init; } = types;
    public override IEnumerable<CompletionResult> CompleteArgument(string commandName,
                                                                   string parameterName,
                                                                   string wordToComplete,
                                                                   CommandAst commandAst,
                                                                   IDictionary fakeBoundParameters)
    {
        var (word, quote, isEmpty) = ParseWord(wordToComplete);
        var availableTypes = new HashSet<ResourceType>();
        foreach (var item in EnumerateCacheItems(ResourceTypes))
        {
            var name = item.ToString(); // "{Type}:{Id}[:{Name}]"
            if (isEmpty
                || name.StartsWith(word, StringComparison.OrdinalIgnoreCase)
                || item.Name.StartsWith(word, StringComparison.OrdinalIgnoreCase))
            {
                yield return new CompletionResult(ToCompletionText(name, quote),
                                                  name,
                                                  CompletionResultType.ParameterValue,
                                                  item.ToTooltip());
                availableTypes.Add(item.Type);
            }
        }
        // complete remaining types
        foreach (var type in ResourceTypes.Where(t => !availableTypes.Contains(t)))
        {
            var name = $"{type}:";
            if (isEmpty || name.StartsWith(word, StringComparison.OrdinalIgnoreCase))
            {
                yield return new CompletionResult(ToCompletionText(name, quote),
                                                  name,
                                                  CompletionResultType.ParameterValue,
                                                  "Incompleted candidate");
            }
        }
    }
}

internal class ResourceIdCompleter(ResourceType[] types) : ResourceCompleterBase
{
    public ResourceIdCompleter(ResourceType[] types, string filterKey, string[] filterValues)
        : this(types)
    {
        FilterKey = filterKey;
        FilterValues = [.. filterValues];
    }
    public ResourceType[] ResourceTypes { get; init; } = types;
    public string? FilterKey { get; }
    public HashSet<string>? FilterValues { get; }

    public override IEnumerable<CompletionResult> CompleteArgument(string commandName,
                                                                   string parameterName,
                                                                   string wordToComplete,
                                                                   CommandAst commandAst,
                                                                   IDictionary fakeBoundParameters)
    {
        var (word, quote, isEmpty) = ParseWord(wordToComplete);
        foreach (var item in EnumerateCacheItems(ResourceTypes, FilterKey, FilterValues))
        {
            var id = $"{item.Id}";
            var name = item.ToString(); // "{Type}:{Id}[:{Name}]"
            if (isEmpty
                || id.StartsWith(word, StringComparison.OrdinalIgnoreCase)
                || name.StartsWith(word, StringComparison.OrdinalIgnoreCase)
                || item.Name.StartsWith(word, StringComparison.OrdinalIgnoreCase))
            {
                yield return new CompletionResult(ToCompletionText(id, quote), $"{id} ({name})",
                                                  CompletionResultType.ParameterValue, item.ToTooltip());
            }
        }
    }
}

internal abstract class ResourceCompleterBase : IArgumentCompleter
{
    protected static (string Word, char? Quote, bool IsEmpty) ParseWord(string wordToComplete)
    {
        if (string.IsNullOrEmpty(wordToComplete))
        {
            return (string.Empty, null, true);
        }
        ReadOnlySpan<char> word = wordToComplete;
        char? quote = null;
        if (word[0] is '\'' or '"')
        {
            quote = word[0];
            word = word.Length > 1 && word[^1] == quote ? word[1..^1] : word[1..];
        }
        return (word.ToString(), quote, word.Length == 0);
    }
    protected static IEnumerable<CacheItem> EnumerateCacheItems(ResourceType[] types,
                                                                string? filterKey = null,
                                                                IEnumerable<string>? filterValues = null)
    {
        return filterKey is null || filterValues is null
            ? Caches.GetEnumerator(types)
            : Caches.GetEnumerator(types)
                    .Where(item => item.Metadata.TryGetValue(filterKey, out var val) && filterValues.Contains(val));

    }
    protected static string ToCompletionText(string text, char? quote = null)
    {
        return quote is null
            ? Utils.QuoteIfNeed(text)
            : Utils.QuoteIfNeed(text, (char)quote, true);
    }
    public abstract IEnumerable<CompletionResult> CompleteArgument(string commandName,
                                                                   string parameterName,
                                                                   string wordToComplete,
                                                                   CommandAst commandAst,
                                                                   IDictionary fakeBoundParameters);
}
