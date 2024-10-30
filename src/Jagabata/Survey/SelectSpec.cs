using System.Collections;
using Jagabata.Resources;

namespace Jagabata.Survey;

public abstract class SelectSpec : SurveySpec
{
    public SelectSpec(SurveySpecType type) : base(type)
    { }
    public SelectSpec(SurveySpecType type, string name, string variableName) : base(type, name, variableName)
    { }
    public SelectSpec(SurveySpecType type, string variableName): this(type, variableName, variableName)
    { }
    public sealed override object Choices
    {
        get { return _choices; }
        set
        {
            switch (value)
            {
                case null:
                    _choices = [];
                    break;
                case string str:
                    _choices = [str];
                    break;
                case IList list:
                    var results = new List<string>();
                    foreach (var item in list)
                    {
                        if (item is null) continue;
                        results.Add($"{item}");
                    }
                    _choices = results.ToArray();
                    break;
                default:
                    throw new InvalidCastException($"{nameof(value)} is must be string or IList: {value.GetType()}");
            }
        }
    }
    protected string[] _choices = [];
}

