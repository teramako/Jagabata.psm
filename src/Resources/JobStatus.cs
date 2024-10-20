using System.Text.Json.Serialization;

namespace Jagabata.Resources
{
    [JsonConverter(typeof(Json.EnumUpperCamelCaseStringConverter<JobStatus>))]
    public enum JobStatus
    {
        New,
        Started,
        Pending,
        Waiting,
        Running,
        Successful,
        Failed,
        Error,
        Canceled,
    }
}

