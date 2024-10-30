using Jagabata.Resources;

namespace Jagabata.Survey;

public sealed class PasswordSpec : InputSpec<string>
{
    public PasswordSpec() : base(SurveySpecType.Password)
    { }
    public PasswordSpec(string name, string variableName) : base(SurveySpecType.Password, name, variableName)
    { }
    public PasswordSpec(string variableName): base(SurveySpecType.Password, variableName)
    { }
}

