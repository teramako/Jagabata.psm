---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Find-AnsibleSystemJob

## SYNOPSIS
Retrieve jobs for SystemJobTemplate.

## SYNTAX

```
Find-AnsibleSystemJob [[-SystemJobTemplate] <UInt64>] [-Status <String[]>] [-OrderBy <String[]>]
 [-Search <String[]>] [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All]
 [<CommonParameters>]
```

## DESCRIPTION
Retrieve the list of jobs for SystemJobTemplate.

Implementation of following API:  
- `/api/v2/system_jobs/`  
- `/api/v2/projects/{id}/project_updates/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Find-AnsibleSystemJob
```

### Example 2
```powershell
PS C:\> Find-AnsibleSystemJob -Type SystemJobTemplate -Id 1
```

Retrieve ProjectUpdate jobs associated with the SystemJobTemplate of ID 1

`Id` and `Type` parameters can also be given from the pipeline, likes following:  
    Get-AnsibleSystemJobTemplate -Id 1 | Find-AnsibleSystemJob

and also can omit `-Type` parameter:  
    Find-AnsibleSystemJob -Id 1

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

### -Status
Filter by `status` field.

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:
Accepted values: new, started, pending, waiting, running, successful, failed, error, canceled

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SystemJobTemplate
SystemJobTemplate ID or it's object.
Retrieve jobs which the SystemJobTemplate associated with.

```yaml
Type: UInt64
Parameter Sets: (All)
Aliases:

Required: False
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### Jagabata.Resources.ResourceType
Input by `Type` property in the pipeline object.

Acceptable values: `SystemJobTemplate` (only)

### System.UInt64
SystemJobTemplate ID or it's object.

## OUTPUTS

### Jagabata.Resources.SystemJob
## NOTES

## RELATED LINKS

[Get-AnsibleSystemJob](Get-AnsibleSystemJob.md)

[Remove-AnsibleSystemJob](Remove-AnsibleSystemJob.md)

[Get-AnsibleSystemJobTemplate](Get-AnsibleSystemJobTemplate.md)

[Find-AnsibleSystemJobTemplate](Find-AnsibleSystemJobTemplate.md)

[Invoke-AnsibleSystemJobTemplate](Invoke-AnsibleSystemJobTemplate.md)

[Start-AnsibleSystemJobTemplate](Start-AnsibleSystemJobTemplate.md)

[Find-AnsibleUnifiedJob](Find-AnsibleUnifiedJob.md)
