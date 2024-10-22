using System.Diagnostics.CodeAnalysis;

namespace Jagabata.Cmdlets;

public abstract class NewCommandBase<TResource> : APICmdletBase where TResource : class
{
    protected abstract Dictionary<string, object> CreateSendData();

    private string? _apiPath = null;
    protected virtual string ApiPath
    {
        get
        {
            if (_apiPath is not null)
                return _apiPath;

            _apiPath = GetApiPath(typeof(TResource));
            return _apiPath;
        }
    }

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
