using Jagabata.Resources;

namespace Jagabata.Survey;

public abstract class InputSpec<T> : SurveySpec
{
    public InputSpec(SurveySpecType type) : base(type)
    { }
    public InputSpec(SurveySpecType type, string name, string variableName) : base(type, name, variableName)
    { }
    public InputSpec(SurveySpecType type, string variableName): this(type, variableName, variableName)
    { }
    public sealed override object? Default
    {
        get { return DefaultValue; }
        set
        {
            if (value is null)
            {
                DefaultValue = default(T);
                return;
            }
            DefaultValue = (T)value;
        }
    }
    protected T? DefaultValue = default(T);
    public sealed override object Choices
    {
        get { return string.Empty; }
        set { throw new InvalidOperationException(""); }
    }
}

