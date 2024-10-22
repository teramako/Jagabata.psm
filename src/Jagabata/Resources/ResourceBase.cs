namespace Jagabata.Resources
{
    public interface IResource
    {
        /// <summary>
        /// Database ID for the resource
        /// </summary>
        ulong Id { get; }
        /// <summary>
        /// Data type for the resource
        /// </summary>
        ResourceType Type { get; }
    }

    record struct Resource(ResourceType Type, ulong Id) : IResource;

    public interface IResource<TSummary> : IResource
    {
        string Url { get; }
        /// <summary>
        /// Data structure with URLs of related resources.
        /// </summary>
        RelatedDictionary Related { get; }
        /// <summary>
        /// Data structure with name/description for related resources.
        /// The output for some objects may be limited for performance reasons.
        /// </summary>
        TSummary SummaryFields { get; }
    }
}
