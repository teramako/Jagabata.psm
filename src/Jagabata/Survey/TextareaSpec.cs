using Jagabata.Resources;

namespace Jagabata.Survey;

public sealed class TextareaSpec : InputSpec<string>
{
    public TextareaSpec() : base(SurveySpecType.Textarea)
    { }
    public TextareaSpec(string name, string variableName) : base(SurveySpecType.Textarea, name, variableName)
    { }
    public TextareaSpec(string variableName) : base(SurveySpecType.Textarea, variableName)
    { }
}

