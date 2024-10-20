namespace Jagabata.Resources
{
    public class Metrics
        : Dictionary<string, Metrics.Item>
    {
        public const string PATH = "/api/v2/metrics/";

        public record Item(string HelpText,
                           string Type,
                           SampleItem[] Samples);
        public record SampleItem(Dictionary<string, string> Labels,
                                 double Value,
                                 string? SampleType);
    }
}
