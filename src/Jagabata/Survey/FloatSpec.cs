using Jagabata.Resources;

namespace Jagabata.Survey;

public sealed class FloatSpec : InputSpec<float>
{
    public FloatSpec() : base(SurveySpecType.Float)
    { }
    public FloatSpec(string name, string variableName) : base(SurveySpecType.Float, name, variableName)
    { }
    public FloatSpec(string variableName) : base(SurveySpecType.Float, variableName)
    { }
}

