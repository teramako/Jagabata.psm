using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jagabata.Resources
{
    /// <summary>
    /// For RestAPI <c>/api/v2/dashboard/graphs/jobs/</c>
    /// </summary>
    /// <remarks>
    /// JSON struct
    /// <code>
    /// {
    ///   "jobs": {
    ///     "successful": [
    ///       [ 1721692800.0, 0 ], // ← UnixEpochTime(double), Count(uint)
    ///       [ 1721779200.0, 1 ],
    ///       ...
    ///     ],
    ///     "failed": [
    ///       [ 1721692800.0, 0 ],
    ///       [ 1721779200.0, 1 ],
    ///       ...
    ///     ]
    ///   }
    /// }
    /// </code>
    /// </remarks>
    public class JobStatisticsContainer(JobStatistics jobs)
    {
        public const string PATH = "/api/v2/dashboard/graphs/jobs/";

        public JobStatistics Jobs { get; } = jobs;
    }

    /// <summary>
    /// Job Statistics Details
    /// </summary>
    /// <param name="successful">Successful job items</param>
    /// <param name="failed">Failed job items</param>
    public class JobStatistics(JobStatistics.Item[] successful, JobStatistics.Item[] failed)
    {
        public Item[] Successful { get; } = successful;
        public Item[] Failed { get; } = failed;

        /// <summary>
        /// JobStatistics item
        /// </summary>
        [JsonConverter(typeof(EpochCountConverter))]
        public readonly struct Item(DateTime date, uint count)
        {
            public DateTime Date { get; } = date;
            public uint Count { get; } = count;

            public override string ToString()
            {
                return $"[ Date = {Date.ToShortDateString()}, Count = {Count} ]";
            }
        };

        /// <summary>
        /// <c>[ UnixEpochTime(double), Count(uint) ]</c> &lt;=&gt;
        /// <see cref="Item">Item</see> Converter
        /// </summary>
        class EpochCountConverter : JsonConverter<Item>
        {
            public override Item Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType != JsonTokenType.StartArray)
                    throw new JsonException();
                reader.Read();

                if (reader.TokenType != JsonTokenType.Number)
                    throw new JsonException();
                double seconds = reader.GetDouble();

                reader.Read();

                if (reader.TokenType != JsonTokenType.Number)
                    throw new JsonException();
                uint count = reader.GetUInt32();

                reader.Read();
                if (reader.TokenType != JsonTokenType.EndArray)
                    throw new JsonException();

                var datetimeOffset = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(seconds));
                return new Item(datetimeOffset.UtcDateTime, count);
            }
            public override void Write(Utf8JsonWriter writer, Item value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                var datetimeOffset = new DateTimeOffset(value.Date);
                writer.WriteNumberValue(datetimeOffset.ToUnixTimeSeconds());
                writer.WriteNumberValue(value.Count);
                writer.WriteEndArray();
            }
        }
    }
}

