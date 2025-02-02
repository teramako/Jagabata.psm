using Jagabata.Resources;

namespace Jagabata.Survey;

public sealed class IntegerSpec : InputSpec<int>
{
    public IntegerSpec() : base(SurveySpecType.Integer)
    { }
    public IntegerSpec(string name, string variableName) : base(SurveySpecType.Integer, name, variableName)
    { }
    public IntegerSpec(string variableName) : base(SurveySpecType.Integer, variableName)
    { }
}

