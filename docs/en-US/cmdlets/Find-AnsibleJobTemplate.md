---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Find-AnsibleJobTemplate

## SYNOPSIS
Retrieve JobTemplates.

## SYNTAX

```
Find-AnsibleJobTemplate [[-Resource] <IResource>] [[-Name] <String[]>] [-OrderBy <String[]>]
 [-Search <String[]>] [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All]
 [<CommonParameters>]
```

## DESCRIPTION
Retrieve the list of JobTemplates.

Implementation of following API:  
- `/api/v2/job_templates/`  
- `/api/v2/organizations/{id}/job_templates/`  
- `/api/v2/inventories/{id}/job_templates/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Find-AnsibleJobTemplate
```

### Example 2
```powershell
PS C:\> Find-AnsibleJobTemplate -Resource Organization:1
```

Retrieve JobTemplates associated with the Organization of ID 1

## PARAMETERS

### -All
Retrieve resources from all pages.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Count
Number to retrieve per page.

```yaml
Type: UInt16
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: 20
Accept pipeline input: False
Accept wildcard characters: False
```

### -Filter
Filtering various fields.

For examples:  
- `name__icontains=test`: "name" field contains "test" (case-insensitive).  
- `"name_ in=test,demo", created _gt=2024-01-01`: "name" field is "test" or "demo" and created after 2024-01-01.  
- `@{ Name = "name"; Value = "test"; Type = "Contains"; Not = $true }`: "name" field NOT contains "test"

For more details, see [about_Jagabata.psm_Filter_parameter](about_Jagabata.psm_Filter_parameter.md).

```yaml
Type: NameValueCollection
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
Filter by JobTemplate name.
The names must be an exact match. (case-sensitive)

Multiple words are available by separating with a comma(`,`).

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -OrderBy
Retrieve list in the specified orders.
Use `!` prefix to sort in reverse.
Multiple sorting fields are available by separating with a comma(`,`).

Default value: `!id` (descending order of ID)

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: ["!id"]
Accept pipeline input: False
Accept wildcard characters: False
```

### -Page
Page number.

```yaml
Type: UInt32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: 1
Accept pipeline input: False
Accept wildcard characters: False
```

### -Resource
Resource object associated with the resource to be find.

The resource is an object with `Id` and `Type` properties.
And `Type` should be following value:  
- `Organization`  
- `Inventory`

> [!TIP]  
> Can specify the resource as string like `Inventory:1` (Format: `{Type}:{Id}`).
> And also accept objects have `type` and `id` properties.  
>
> For example:  
>  - `-Resource (Get-AnsibleInventory -Id 1)`  
>  - `-Resource @{ type = "inventory"; id = 1 }`  
>  - `-Resource inventory:1`

```yaml
Type: IResource
Parameter Sets: (All)
Aliases:

Required: False
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Search
Search words. (case-insensitive)

Target fields: `name`, `description`

Multiple words are available by separating with a comma(`,`).

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### Jagabata.Resources.IResource
Resource object to which the search target associated with.
See: `-Resource` parameter.

## OUTPUTS

### Jagabata.Resources.JobTemplate
## NOTES

## RELATED LINKS

[Get-AnsibleJobTemplate](Get-AnsibleJobTemplate.md)

[New-AnsibleJobtemplate](New-AnsibleJobTemplate.md)

[Update-AnsibleJobTemplate](Update-AnsibleJobTemplate.md)

[Remove-AnsibleJobTemplate](Remove-AnsibleJobTemplate.md)

[Find-AnsibleUnifiedJobTemplate](Find-AnsibleUnifiedJobTemplate.md)

[Invoke-AnsibleJobTemplate](Invoke-AnsibleJobTemplate.md)

[Start-AnsibleJobTemplate](Start-AnsibleJobTemplate.md)

[Find-AnsibleJob](Find-AnsibleJob.md)
