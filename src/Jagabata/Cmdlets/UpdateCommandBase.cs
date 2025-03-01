using System.Diagnostics.CodeAnalysis;

namespace Jagabata.Cmdlets;

public abstract class UpdateCommandBase<TResource> : APICmdletBase where TResource : class
{
    public abstract ulong Id { get; set; }

    protected abstract Dictionary<string, object?> CreateSendData();

    private string? _apiPath;
    protected virtual string ApiPath => _apiPath is not null || Utils.TryGetApiPath<TResource>(out _apiPath)
        ? _apiPath
        : throw new NotImplementedException($"'PATH' field is not implemented on {typeof(TResource)}");

    protected bool TryPatch(ulong id, [MaybeNullWhen(false)] out TResource result)
    {
        result = default;
        var sendData = CreateSendData();
        if (sendData.Count == 0)
        {
            WriteWarning("Send data is empty. Do nothing");
            return false; // do nothing
        }

        var dataDescription = Json.Stringify(sendData, pretty: true);
        if (ShouldProcess($"{typeof(TResource).Name} [{id}]", $"Update {dataDescription}"))
        {
            result = PatchResource<TResource>($"{ApiPath}{id}/", sendData);
            return true;
        }
        return false;
    }
}
