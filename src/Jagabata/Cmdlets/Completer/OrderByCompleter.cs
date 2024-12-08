using System.Collections;
using System.Management.Automation;
using System.Management.Automation.Language;

namespace Jagabata.Cmdlets.Completer;

internal class OrderByCompleter : IArgumentCompleter
{
    public OrderByCompleter(params string[] keys)
    {
        Keys = keys;
    }
    public string[] Keys { get; init; }
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
