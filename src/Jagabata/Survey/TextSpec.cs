using Jagabata.Resources;

namespace Jagabata.Survey;

public sealed class TextSpec : InputSpec<string>
{
    public TextSpec() : base(SurveySpecType.Text)
    { }
    public TextSpec(string name, string variableName) : base(SurveySpecType.Text, name, variableName)
    { }
    public TextSpec(string variableName): base(SurveySpecType.Text, variableName)
    { }
}

