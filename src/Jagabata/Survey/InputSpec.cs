using Jagabata.Resources;

namespace Jagabata.Survey;

public abstract class InputSpec<T> : SurveySpec
{
    public InputSpec(SurveySpecType type) : base(type)
    { }
    public InputSpec(SurveySpecType type, string name, string variableName) : base(type, name, variableName)
    { }
    public InputSpec(SurveySpecType type, string variableName) : this(type, variableName, variableName)
    { }
    public sealed override object? Default
    {
        get => DefaultValue;
        set
        {
            if (value is null)
            {
                DefaultValue = default;
                return;
            }
            DefaultValue = (T)value;
        }
    }
    private T? DefaultValue;
    public sealed override object Choices
    {
        get => string.Empty; set => throw new InvalidOperationException("");
    }
}

