using System.Text;

namespace Jagabata.Resources
{
    public class ApiHelp(string name,
                         string description,
                         string[] renders,
                         string[] parses,
                         Dictionary<string, ApiHelp.ActionDictionary>? actions,
                         string[]? types,
                         string[]? searchFields,
                         string[]? relatedSearchFields,
                         string[]? objectRoles,
                         uint? maxPageSize)
    {
        public string Name { get; } = name ?? string.Empty;
        public string Description { get; } = description ?? string.Empty;
        public string[] Renders { get; } = renders;
        public string[] Parses { get; } = parses;
        public Dictionary<string, ActionDictionary>? Actions { get; } = actions;
        public string[]? Types { get; } = types;

        public string[]? SearchFields { get; } = searchFields;
        public string[]? RelatedSearchFields { get; } = relatedSearchFields;
        public string[]? ObjectRoles { get; } = objectRoles;
        public uint? MaxPageSize { get; } = maxPageSize;

        public class ActionDictionary : Dictionary<string, FieldDescription>
        {
        }
        public class FieldDescription(string label,
                                      string type,
                                      bool required = false,
                                      bool filterable = false,
                                      int? maxLength = null,
                                      object? @default = null,
                                      string helpText = "")
        {
            public string Label { get; } = label;
            public string Type { get; } = type;
            public bool Required { get; } = required;
            public bool Filterable { get; } = filterable;
            public int? MaxLength { get; } = maxLength;
            public object? Default { get; } = @default;
            public string HelpText { get; } = helpText;
            public override string ToString()
            {
                var culture = System.Globalization.CultureInfo.InvariantCulture;
                var sb = new StringBuilder();
                sb.Append('{')
                  .Append(culture, $" Label = {Label}")
                  .Append(culture, $", Type = {Type}");
                if (Required)
                    sb.Append(culture, $", Required");
                if (Filterable)
                    sb.Append(culture, $", Filterable");
                if (MaxLength is not null)
                    sb.Append(culture, $", MaxLength = {MaxLength}");
                if (Default is not null)
                    sb.Append(culture, $", Default = `{Default}`");
                if (!string.IsNullOrEmpty(HelpText))
                    sb.Append(culture, $", HelpText = {HelpText}");
                sb.Append(" }");
                return sb.ToString();
            }
        }
    }
}
