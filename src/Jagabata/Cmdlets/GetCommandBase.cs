using System.Collections.Specialized;
using System.Management.Automation;
using System.Web;
using Jagabata.Resources;

namespace Jagabata.Cmdlets;

public abstract class GetCommandBase<TResource> : APICmdletBase where TResource : class
{
    public virtual ulong[] Id { get; set; } = [];

    [Parameter(ValueFromPipelineByPropertyName = true, DontShow = true)]
    public ResourceType? Type { get; set; }

    protected HashSet<ulong> IdSet { get; } = [];
    protected NameValueCollection Query { get; } = HttpUtility.ParseQueryString("");

    private string? _apiPath;
    protected virtual string ApiPath
    {
        get
        {
            if (_apiPath is not null)
            {
                return _apiPath;
            }

            _apiPath = GetApiPath(typeof(TResource));
            return _apiPath;
        }
        set => _apiPath = value;
    }

    /// <summary>
    /// Gather resource IDs to retrieve
    /// Primarily called from within the <see cref="Cmdlet.ProcessRecord"/> method
    /// </summary>
    protected void GatherResourceId()
    {
        foreach (var id in Id.Where(static id => id > 0))
        {
            IdSet.Add(id);
        }
    }

    /// <summary>
    /// Retrieve and output the resource for the gathered ID.
    /// Primarily called from within the <see cref="Cmdlet.EndProcessing"/> method
    /// </summary>
    protected IEnumerable<TResource> GetResultSet()
    {
        if (IdSet.Count == 0)
        {
            yield break;
        }
        else if (IdSet.Count == 1)
        {
            var res = GetResource<TResource>($"{ApiPath}{IdSet.First()}/");
            yield return res;
        }
        else
        {
            Query.Add("id__in", string.Join(',', IdSet));
            Query.Add("page_size", $"{IdSet.Count}");
            foreach (var resultSet in GetResultSet<TResource>(ApiPath, Query, true))
            {
                foreach (var res in resultSet.Results)
                {
                    yield return res;
                }
            }
        }
    }

    /// <summary>
    /// Get and output resources individually.
    /// Primarily called from within the <see cref="Cmdlet.ProcessRecord"/> method
    /// </summary>
    /// <param name="subPath">sub path</param>
    protected IEnumerable<TResource> GetResource(string subPath = "")
    {
        foreach (var id in Id.Where(static id => id > 0))
        {
            if (!IdSet.Add(id))
            {
                // skip already processed
                continue;
            }
            var res = GetResource<TResource>($"{ApiPath}{id}/{subPath}");
            yield return res;
        }
    }
}
