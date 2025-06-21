using System.Diagnostics.CodeAnalysis;

namespace Jagabata.Cmdlets;

public abstract class NewCommandBase<TResource> : APICmdletBase where TResource : class
{
    protected abstract Dictionary<string, object> CreateSendData();

    private string? _apiPath;
    protected virtual string ApiPath => _apiPath is not null || Utils.TryGetApiPath<TResource>(out _apiPath)
        ? _apiPath
        : throw new NotImplementedException($"'PATH' field is not implemented on {typeof(TResource)}");

    protected bool TryCreate([MaybeNullWhen(false)] out TResource result, string? action = null)
    {
        return TryCreate(ApiPath, out result, action);
    }
    protected bool TryCreate(string path, [MaybeNullWhen(false)] out TResource result, string? action = null)
    {
        result = default;
        var sendData = CreateSendData();
        var dataDescription = Json.Stringify(sendData, pretty: true);
        if (action is null
                ? ShouldProcess(dataDescription)
                : ShouldProcess(dataDescription, action))
        {
            var apiResult = CreateResource<TResource>(path, sendData);
            result = apiResult.Contents;
            return result is not null;
        }
        return false;
    }
}
