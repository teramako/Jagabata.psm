using System.Collections;
using System.Management.Automation;
using System.Management.Automation.Language;

namespace Jagabata.Cmdlets.Completer;

internal class OrderByCompleter(params string[] keys) : IArgumentCompleter
{
    public string[] Keys { get; init; } = keys;
    public IEnumerable<CompletionResult> CompleteArgument(string commandName, string parameterName,
                                                          string wordToComplete, CommandAst commandAst,
                                                          IDictionary fakeBoundParameters)
    {
        var word = wordToComplete.StartsWith('!')
                   ? wordToComplete[1..].ToLowerInvariant()
                   : wordToComplete.ToLowerInvariant();
        foreach (var key in Keys)
        {
            if (!key.StartsWith(word, StringComparison.InvariantCulture))
            {
                continue;
            }

            yield return new CompletionResult(key, key, CompletionResultType.Keyword, $"Order by {key} ascending");
            var descendingProp = '!' + key;
            yield return new CompletionResult(descendingProp, descendingProp, CompletionResultType.Keyword, $"Order by {key} descending");
        }
    }
}
internal class OrderByCompleterFromHelp(ResourceType resourceType, string path) : IArgumentCompleter
{
    public IEnumerable<CompletionResult> CompleteArgument(string commandName, string parameterName,
                                                          string wordToComplete, CommandAst commandAst,
                                                          IDictionary fakeBoundParameters)
    {
        if (!Caches.ApiHelps.TryGetValue(resourceType, out var help))
        {
            var apiResult = RestAPI.OptionsJsonAsync<Resources.ApiHelp>(path).GetAwaiter().GetResult();
            help = apiResult.Contents;
            Caches.ApiHelps[resourceType] = help;
        }
        if (help.Actions is null || !help.Actions.TryGetValue("GET", out var actions))
            yield break;

        var word = wordToComplete.StartsWith('!')
                   ? wordToComplete[1..].ToLowerInvariant()
                   : wordToComplete.ToLowerInvariant();
        foreach (var (key, _) in actions.Where(kv => kv.Value.Filterable
                                                  && kv.Key.StartsWith(word, StringComparison.Ordinal)))
        {
            yield return new(key, key, CompletionResultType.Keyword, $"Order by \"{key}\" ascending");
            var descendingKey = $"!{key}";
            yield return new(descendingKey, descendingKey, CompletionResultType.Keyword, $"Order by \"{key}\" descending");
        }
    }
}
