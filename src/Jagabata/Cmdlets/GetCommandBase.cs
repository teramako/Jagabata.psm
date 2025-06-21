using System.Management.Automation;

namespace Jagabata.Cmdlets;

public abstract class GetCommandBase<TResource> : APICmdletBase where TResource : class
{
    public virtual ulong[] Id { get; set; } = [];

    protected HashSet<ulong> IdSet { get; } = [];
    protected HttpQuery Query { get; } = [];

    private string? _apiPath;
    protected virtual string ApiPath
    {
        get => _apiPath is not null || Utils.TryGetApiPath<TResource>(out _apiPath)
              ? _apiPath
              : throw new NotImplementedException($"'PATH' field is not implemented on {typeof(TResource)}");
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
        return IdSet.Count switch
        {
            0 => [],
            1 => [GetResource<TResource>($"{ApiPath}{IdSet.First()}/")],
            _ => new QueryBuilder(Query).SetOrderBy("id")
                                        .BuildWithIdList(IdSet.Order().ToArray())
                                        .SelectMany(query => GetResultSet<TResource>(ApiPath, query))
                                        .SelectMany(static resultSet => resultSet.Results)
        };
    }

    /// <summary>
    /// Get and output resources individually.
    /// Primarily called from within the <see cref="Cmdlet.ProcessRecord"/> method
    /// </summary>
    /// <param name="subPath">sub path</param>
    protected IEnumerable<TResource> GetResource(string subPath = "")
    {
        if (!string.IsNullOrEmpty(subPath) && !subPath.EndsWith('/'))
            subPath += '/';

        var tailingPathAndQuery = Query.Count == 0 ? subPath : $"{subPath}?{Query}";

        foreach (var id in Id.Where(static id => id > 0))
        {
            if (!IdSet.Add(id))
            {
                // skip already processed
                continue;
            }
            var res = GetResource<TResource>($"{ApiPath}{id}/{tailingPathAndQuery}");
            yield return res;
        }
    }
}
