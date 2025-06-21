using System.Management.Automation;

namespace Jagabata.Cmdlets;

public abstract class RemoveCommandBase<TResource> : APICmdletBase
{
    [Parameter()]
    public SwitchParameter Force { get; set; }

    private string? _apiPath;
    protected virtual string ApiPath => _apiPath is not null || Utils.TryGetApiPath<TResource>(out _apiPath)
        ? _apiPath
        : throw new NotImplementedException($"'PATH' field is not implemented on {typeof(TResource)}");

    protected bool TryDelete(ulong id, string? target = null)
    {
        return TryDelete(ApiPath, id, target);
    }

    protected bool TryDelete(string path, ulong id, string? target = null)
    {
        if (Force || ShouldProcess(target is not null ? target : $"{typeof(TResource).Name} [{id}]"))
        {
            var apiResult = DeleteResource($"{path}{id}/");
            var isSuccess = apiResult?.IsSuccessStatusCode ?? false;
            if (isSuccess)
            {
                WriteVerbose($"{typeof(TResource).Name} [{id}] is removed.");
            }
            return isSuccess;
        }
        return false;
    }
}

