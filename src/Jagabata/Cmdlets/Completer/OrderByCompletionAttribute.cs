using System.Management.Automation;

namespace Jagabata.Cmdlets.Completer;

internal class OrderByCompletionAttribute(params string[] keys)
    : ArgumentCompleterAttribute, IArgumentCompleterFactory
{
    public string[] Keys { get; init; } = keys;
    public IArgumentCompleter Create()
    {
        return new OrderByCompleter(Keys);
    }
}
