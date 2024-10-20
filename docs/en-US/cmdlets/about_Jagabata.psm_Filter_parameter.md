# About `-Filter` parameter
About `-Filter` parameter on Jagabata.psm's cmdlets.

Many command, especially those with "File-" have `-Filter` parameter.
This allows you to filter the reults using a various set of contiditions.

See [Filtering — Automation Controller API Guide](https://docs.ansible.com/automation-controller/latest/html/controllerapi/filtering.html)
for specification on what kind of fitering is available.

## Parameter values

`-Filter` parameter accepts one or more values separated by commas (`,`).

For example:

```powershell
Find-xyz -Filter field1=value1, "field2=value2&field3=value3", @{ name="field4"; value="value4" }
```

A parameter value can be accepted following types:

### Type "string"

A `key=value` format string, or multiple the formated values combined with `&`.
It's means that `string` parameter value is same as HTTP URL query.

However, `&` has a special meaning in PowerShell, so if you use `&`, you should quote `"` or `'`.

All `key` values are coverted to lowwercase.

#### For example:

- `field=xyz`.
- `"field=xyz&created__gt=2024-01-01"` or `field=xyz, created_gt=2024-01-1`; example with multiple parameter values.
- `FIELD=xyz`; same as `field=xyz`; field name is converted to lowercase.

### Type "NameValueCollection"

Especially, `NameValueCollection` created by `System.Web.HttpUtility.ParseQueryString(...)`.

The specification is almost same as "Type string".

#### For example:

- `([Web.HttpUtility]::ParseQueryString("field=xyz"))`.
- `([Web.HttpUtility]::ParseQueryString("field=xyz&created__gt=2024-01-01"))`.

### Type "IDictionary"

Accepts `IDictionary` type like `Hashtable` or `OrderedDictionary` for easy conversion from PowerShell to the following `Jagabata.Cmdlets.Filter` type.

#### For example:

- `@{ name = "field"; value = "xyz" }` => `field=xyz`.
- `@{ NAME = "FIELD"; VALUE = "XYZ" }` => `field=XYZ`; Dictionary Keys are case-insensitive, and `Name` property value is converted to lowercase.
- `@{ name = "Field"; value = "xyz"; type = "Contains" }` => `field__contains=xyz`.
- `@{ name = "field__contains"; value = "xyz" }` => `field__contains=xyz`; same as above.
- `@{ name = "field"; value = "xyz"; or = $true }` => `or__field__contains=xyz`.
- `@{ name = "or__field"; value = "xyz" }` => `or__field__contains=xyz`; same as above.
- `@{ name = "field"; value = "xyz"; not = $true }` => `not__field__contains=xyz`.
- `@{ name = "not__field"; value = "xyz"; not = $true }` => `not__field__contains=xyz`; same as above.
- `@{ name = "created"; value = Get-Date 2024-01-01; type = "gt" }` => `creeated__gt=2024-01-01T00:00:00.0000000{TimeZone}`; `DateTime` object is converted to ISO Date string.
- `@{ name = "id__in"; value = 1,2,3 }` => `id__in=1,2,3`; `IList` object is converted to commas separeted string.

### Type "Jagabata.Cmdlets.Filter"

This is the most primitive object of this filtering mechanism.
All of the above types, `string`, `NameValueCollection` and `IDictionary`, are converted once into this `Jagabata.Cmdlets.Filter` type.

This type has following properties:

- `Name`: (string) The field name to be filtered
- `Value`: (string) The field value to be filtered
- `Type`: (Enum) Lookup Type
  - `Excat` : Exact match (default lookup if not specified)
  - `IExact` : Case-insensitive version of `Exact`.
  - `Contains` : Field contains value.
  - `IContains` : Case-insensitive version of `Contains`.
  - `StartsWith` : Field starts with value.
  - `IStartsWith` : Case-insensitive version of `StartsWith`.
  - `EndsWith` : Field ends with value.
  - `IEndsWith` : Case-insensitive version of `EndsWith`.
  - `Regex` : Field matches the given regular expression.
  - `IRegex` : Case-insensitive version of `Regex`.
  - `GreaterThan`, `GT` : Greater than comparision.
  - `GreaterThanOrEqual`, `GTE` : Greater than or equal comparision.
  - `LessThan`, `LT `: Less than comparision.
  - `LessThanOrEqual`, `LTE` : Less than or equal comparision.
  - `IsNull` : Check whether the given field or selected object is null; expectes a boolean value.
  - `In` : Check whether the given field's value is present in the list provided; expects a list of items.
- `Or`: (bool) Whether "or__" prefix is appended or not
- `Not`: (bool) Whether "not__" prefix is appended or not

#### For example:

- `[Jagabata.Cmdlets.Filter]::new("field", "xyz")` => `field=xyz`.
- `[Jagabata.Cmdlets.Filter]::new("field", "xyz", "icontains")` => `field_icontains=xyz`.
- `[Jagabata.Cmdlets.Filter]::new("field", "xyz", "icontains", $true)` => `or__field__icontains=xyz`.
- `[Jagabata.Cmdlets.Filter]::new("field", "xyz", "icontains", $true, $true)` => `or__not__field__icontains=xyz`.
- `[Jagabata.Cmdlets.Filter]::new("field", "xyz", "icontains", $false, $true)` => `not__field__icontains=xyz`.
- `[Jagabata.Cmdlets.Filter]::new("field__icontains", "xyz")` => `field__icontains=xyz`.
- `[Jagabata.Cmdlets.Filter]::new("or__field__icontains", "xyz")` => `or__field__icontains=xyz`.
- `[Jagabata.Cmdlets.Filter]::new("or__not__field__icontains", "xyz")` => `or__not__field__icontains=xyz`.
