namespace Jagabata.Resources
{
    /// <summary>
    /// <code>
    /// /api/v2/settings/
    /// </code>
    /// </summary>
    public class Setting
    {
        public Setting(string url, string slug, string name)
        {
            Url = url;
            Slug = slug;
            Name = name;
        }

        public string Url { get; }
        public string Slug { get; }
        public string Name { get; }
    }
}

