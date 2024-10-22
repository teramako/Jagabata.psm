using System.Text.Json.Serialization;

namespace Jagabata.Resources
{
    [JsonConverter(typeof(Json.EnumUpperCamelCaseStringConverter<JobType>))]
    public enum JobType
    {
        Run,
        Check,
        Scan
    }
}

