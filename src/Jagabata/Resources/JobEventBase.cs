namespace Jagabata.Resources
{
    public interface IJobEventBase
    {
        DateTime Created { get; }
        DateTime? Modified { get; }
        JobEventEvent Event { get; }
        int Counter { get; }
        string EventDisplay { get; }
        Dictionary<string, object?> EventData { get; }
        bool Failed { get; }
        bool Changed { get; }
        string UUID { get; }
        string Stdout { get; }
        int StartLine { get; }
        int EndLine { get; }
        JobVerbosity Verbosity { get; }
    }

    public abstract class JobEventBase : ResourceBase, IJobEventBase
    {
        public abstract DateTime Created { get; }
        public abstract DateTime? Modified { get; }
        public abstract JobEventEvent Event { get; }
        public abstract int Counter { get; }
        public abstract string EventDisplay { get; }
        public abstract Dictionary<string, object?> EventData { get; }
        public abstract bool Failed { get; }
        public abstract bool Changed { get; }
        public abstract string UUID { get; }
        public abstract string Stdout { get; }
        public abstract int StartLine { get; }
        public abstract int EndLine { get; }
        public abstract JobVerbosity Verbosity { get; }

        protected override CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, string.Empty, $"{Counter}:{Event}")
            {
                Metadata = {
                    ["Failed"] = $"{Failed}",
                    ["Changed"] = $"{Changed}",
                }
            };
        }

        public override string ToString()
        {
            return $"{Type}:{Id}:{Counter}:{Event}";
        }
    }
}
