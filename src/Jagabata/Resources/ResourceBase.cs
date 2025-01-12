using System.Diagnostics.CodeAnalysis;

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

    public record struct Resource(ResourceType Type, ulong Id) : IResource, IParsable<Resource>
    {
        public static Resource Parse(string s, IFormatProvider? provider = null)
        {
            return TryParse(s, provider, out var result)
                   ? result
                   : throw new FormatException($"Resource format should be '{{Type}}:{{Id}}': {s}");
        }

        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Resource result)
        {
            result = default;
            if (s is null)
            {
                return false;
            }
            var list = s.Split([':', '#'], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
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
}
