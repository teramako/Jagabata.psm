using System.Diagnostics.CodeAnalysis;
using Jagabata.Resources;

namespace Jagabata
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

    public record struct Resource(ResourceType Type, ulong Id) : IResource, IParsable<Resource>
    {
        public static Resource Parse(string s, IFormatProvider? provider = null)
        {
            return TryParse(s, provider, out var result)
                   ? result
                   : throw new FormatException($"Resource format should be '{{Type}}:{{Id}}': {s}");
        }

        private static readonly char[] separator = [':', '#'];
        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Resource result)
        {
            result = default;
            if (s is null)
            {
                return false;
            }
            var list = s.Split(separator, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (list.Length > 1
                && Enum.TryParse<ResourceType>(list[0], true, out var resourceType)
                && ulong.TryParse(list[1], System.Globalization.NumberStyles.Integer, provider, out var id))
            {
                result = new Resource(resourceType, id);
                return true;
            }
            return false;
        }
        public override readonly string ToString()
        {
            return $"{Type}:{Id}";
        }
    }

    public abstract class ResourceBase : IResource
    {
        public abstract ulong Id { get; }
        public abstract ResourceType Type { get; }
        /// <summary>
        /// URL for this resource
        /// </summary>
        public abstract string Url { get; }
        /// <summary>
        /// Data structure with URLs of related resources.
        /// </summary>
        public abstract RelatedDictionary Related { get; }
        /// <summary>
        /// Data structure with name/description for related resources.
        /// The output for some objects may be limited for performance reasons.
        /// </summary>
        public abstract SummaryFieldsDictionary SummaryFields { get; }

        public override string ToString()
        {
            return $"{Type}:{Id}";
        }
    }
}
