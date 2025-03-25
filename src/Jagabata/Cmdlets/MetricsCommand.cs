using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    public class MetricItem(string name, string description, string type, Dictionary<string, string> labels, double value, string? sampleType)
    {
        public string Name { get; } = name;
        public string Description { get; } = description;
        public string Type { get; } = type;
        public Dictionary<string, string> Labels { get; } = labels;
        public double Value { get; } = value;
        public string? SampleType { get; } = sampleType;
    };

    [Cmdlet(VerbsCommon.Get, "Metric")]
    [OutputType(typeof(MetricItem))]
    public class GetMetricCommand : APICmdletBase
    {
        protected override void ProcessRecord()
        {
            var metrics = GetResource<Metrics>(Metrics.PATH);
            foreach (var (key, item) in metrics)
            {
                WriteObject(CreateItem(key, item), true);
            }
        }
        private static IEnumerable<MetricItem> CreateItem(string key, Metrics.Item item)
        {
            foreach (var sample in item.Samples)
            {
                yield return new MetricItem(key, item.HelpText, item.Type, sample.Labels, sample.Value, sample.SampleType);
            }
        }
    }
}
