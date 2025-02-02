using System.Collections;
using Jagabata.Resources;

namespace Jagabata.Survey;

public class MultiSelectSpec : SelectSpec
{
    public MultiSelectSpec() : base(SurveySpecType.MultiSelect)
    { }
    public MultiSelectSpec(string name, string variableName) : base(SurveySpecType.MultiSelect, name, variableName)
    { }
    public MultiSelectSpec(string variableName) : base(SurveySpecType.MultiSelect, variableName)
    { }
    public override object? Default
    {
        get => string.Join('\n', _defaults);
        set
        {
            switch (value)
            {
                case null:
                    _defaults = [];
                    break;
                case string str:
                    _defaults = str.Split('\n');
                    foreach (var item in _defaults)
                    {
                        if (!_choices.Any(c => c == item))
                        {
                            throw new InvalidDataException($"\"{item}\" is must be one of the Choices values [{string.Join(", ", _choices.Select(c => $"\"{c}\""))}]");
                        }
                    }
                    break;
                case IList list:
                    var results = new List<string>();
                    foreach (var item in list)
                    {
                        if (item is null)
                        {
                            continue;
                        }

                        string val = $"{item}";
                        if (_choices.Any(c => c == val))
                        {
                            results.Add(val);
                        }
                        else
                        {
                            throw new InvalidDataException($"\"{val}\" is must be one of the Choices values [{string.Join(", ", _choices.Select(c => $"\"{c}\""))}]");
                        }
                    }
                    _defaults = [.. results];
                    break;
                default:
                    throw new InvalidCastException($"{nameof(value)} is must be string or IList: {value.GetType()}");
            }
        }
    }
    private string[] _defaults = [];
}

