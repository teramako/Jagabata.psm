namespace Jagabata.Resources
{
    public class LaunchedBy(ulong? id, ResourceType type, string name, string url)
    {
        public ulong? Id { get; } = id;
        public ResourceType Type { get; } = type;
        public string Name { get; } = name;
        public string Url { get; } = url;
        public override string ToString()
        {
            return $"{Type}:{Id}:{Name}";
        }
    }
}

