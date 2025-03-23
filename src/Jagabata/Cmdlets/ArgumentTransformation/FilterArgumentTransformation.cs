using System.Collections;
using System.Collections.Specialized;
using System.Management.Automation;

namespace Jagabata.Cmdlets.ArgumentTransformation
{
    /// <summary>
    /// Transform <c>-Filter</c> parameter values to <see cref="NameValueCollection"/>.
    /// <br/>
    /// Convertable values are one or more of the following items:<br/>
    /// <list type="bullet">
    ///     <item>
    ///         <term><see cref="Filter"/></term>
    ///         <description>
    ///             <c>[AWX.Cmdlets.Filter]::new("name", "value")</c>,
    ///             <c>[AWX.Cmdlets.Filter]::new("name", "value", "startswith", $true, $true)</c>
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term><c>IDictionary</c>(<c>Hashtable</c>)</term>
    ///         <description>
    ///             <c>@{ name = "name"; value = "value" }</c>,
    ///             <c>@{ name = "name"; value = "value"; type = "startswith"; or = $true; not = $true }</c>
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term><c>string</c></term>
    ///         <description>
    ///             <c>name=value</c>,
    ///             <c>or__not__name__startswith=value</c>,
    ///             <c>name1=value1&amp;name2=value2</c>
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term><c>NameValueCollection</c></term>
    ///         <description>
    ///             <c>[Web.HttpUtility]::ParseQueryString("...")</c>
    ///         </description>
    ///     </item>
    /// </list>
    /// </summary>
    internal class FilterArgumentTransformationAttribute : ArgumentTransformationAttribute
    {
        public override object Transform(EngineIntrinsics engineIntrinsics, object inputData)
        {
            var queryBuilder = new QueryBuilder();
            foreach (var item in ToItems(inputData))
            {
                switch (item)
                {
                    case Filter filter:
                        queryBuilder.Add(filter);
                        continue;
                    case IDictionary dict:
                        queryBuilder.Add(dict);
                        continue;
                    case NameValueCollection nvc:
                        queryBuilder.Add(nvc);
                        continue;
                    case string str:
                        foreach (var kv in str.Split('&'))
                        {
                            queryBuilder.Add(kv);
                        }
                        continue;
                    default:
                        continue;
                }
            }
            return queryBuilder.Build();
        }
        private static IEnumerable ToItems(object inputData)
        {
            if (inputData is IList list)
            {
                foreach (var item in list)
                {
                    yield return item;
                }
            }
            else
            {
                yield return inputData;
            }
        }
    }
}
