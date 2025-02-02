using System.Text.Json;

namespace Jagabata.Resources
{
    public abstract class ResultSetBase(int count, string? next, string? previous)
    {
        public int Count { get; } = count;
        public string? Next { get; } = next;
        public string? Previous { get; } = previous;
    }

    public class ResultSet(int count, string? next, string? previous, object[] results)
        : ResultSetBase(count, next, previous)
    {
        public object?[] Results { get; } = [.. results.OfType<JsonElement>().Select(static json => Json.ObjectToInferredType(json, true))];
    }

    public class ResultSet<T>(int count, string? next, string? previous, T[] results)
        : ResultSetBase(count, next, previous)
    {
        public T[] Results { get; } = results;
    }
}
