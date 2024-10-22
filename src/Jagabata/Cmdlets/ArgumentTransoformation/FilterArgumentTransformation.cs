using System.Collections;
using System.Collections.Specialized;
using System.Management.Automation;
using System.Web;

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
    public class FilterArgumentTransformationAttribute : ArgumentTransformationAttribute
    {
        public override object Transform(EngineIntrinsics engineIntrinsics, object inputData)
        {
            var queries = HttpUtility.ParseQueryString("");

            foreach (var item in ToItems(inputData))
            {
                switch (item)
                {
                    case Filter filter:
                        queries.Add(filter.GetKey(), filter.GetValue());
                        continue;
                    case IDictionary dict:
                        {
                            var f = Filter.Parse(dict);
                            queries.Add(f.GetKey(), f.GetValue());
                        }
                        continue;
                    case NameValueCollection nvc:
                        foreach (var f in GetQueries(nvc))
                        {
                            queries.Add(f.GetKey(), f.GetValue());
                        }
                        continue;
                    case string str:
                        foreach (var f in GetQueries(HttpUtility.ParseQueryString(str)))
                        {
                            queries.Add(f.GetKey(), f.GetValue());
                        }
                        continue;
                    default:
                        continue;
                }
            }
            return queries;
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
        private static IEnumerable<Filter> GetQueries(NameValueCollection collection)
        {
            foreach (string key in collection.Keys)
            {
                var values = collection.GetValues(key);
                if (values is null) continue;
                foreach (var val in values)
                {
                    yield return new Filter(key, val);
                }
            }
        }
    }
}
