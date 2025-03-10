using Jagabata.Resources;

namespace Jagabata.Survey;

public class ChoiceSpec : SelectSpec
{
    public ChoiceSpec() : base(SurveySpecType.MultipleChoice)
    { }
    public ChoiceSpec(string name, string variableName) : base(SurveySpecType.MultipleChoice, name, variableName)
    { }
    public ChoiceSpec(string variableName) : base(SurveySpecType.MultipleChoice, variableName)
    { }
    public override object? Default
    {
        get => _default;
        set
        {
            if (value is null)
            {
                _default = null;
            }
            else
            {
                string val = $"{value}";
                _default = _choices.Any(item => item == val)
                    ? val
                    : throw new InvalidOperationException($"\"{val}\" is must be one of the Choices [{string.Join(", ", _choices.Select(x => $"\"{x}\""))}]");
            }
        }
    }
    private string? _default;
}

