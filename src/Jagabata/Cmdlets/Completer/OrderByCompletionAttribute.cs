using System.Management.Automation;

namespace Jagabata.Cmdlets.Completer;

internal class OrderByCompletionAttribute : ArgumentCompleterAttribute, IArgumentCompleterFactory
{
    public string[] Keys { get; init; } = [];
    public IArgumentCompleter Create()
    {
        return new OrderByCompleter(Keys);
    }
}
