namespace Jagabata.Resources
{
    /// <summary>
    /// <code>
    /// /api/v2/settings/
    /// </code>
    /// </summary>
    public class Setting(string url, string slug, string name)
    {
        public string Url { get; } = url;
        public string Slug { get; } = slug;
        public string Name { get; } = name;
    }
}

