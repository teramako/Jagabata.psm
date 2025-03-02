namespace Jagabata.Resources;

public abstract class SummaryFieldsContainer : ResourceBase, IHasCacheableItems
{
    public virtual IEnumerable<CacheItem> GetCacheableItems()
    {
        foreach (var summaryItem in SummaryFields.Values)
        {
            switch (summaryItem)
            {
                case Array arr:
                    foreach (var item in arr.OfType<ICacheableResource>())
                    {
                        yield return item.GetCacheItem();
                    }
                    continue;
                case ListSummary<ICacheableResource> list:
                    foreach (var item in list.Results)
                    {
                        yield return item.GetCacheItem();
                    }
                    continue;
                case ICacheableResource res:
                    yield return res.GetCacheItem();
                    continue;
                default:
                    continue;
            }
        }
    }
}
