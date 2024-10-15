---
external help file: AWX.psm.dll-Help.xml
Module Name: AWX.psm
online version:
schema: 2.0.0
---

# Find-Job

## SYNOPSIS
Retrieve jobs for JobTemplate.

## SYNTAX

```
Find-Job [[-Id] <UInt64>] [[-Name] <String[]>] [-Status <String[]>] [-LaunchType <String[]>]
 [-OrderBy <String[]>] [-Search <String[]>] [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>]
 [-All] [<CommonParameters>]
```

## DESCRIPTION
Retrieve the list of jobs launched from JobTemplates.

Implementation of following API:  
- `/api/v2/jobs/`  
- `/api/v2/job_templates/{id}/jobs/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Find-Job -Status running
```

Retreive running jobs.

### Example 2
```powershell
PS C:\> Find-Job -Type JobTemplate -Id 1
```

Retrieve jobs associated with the JobTemplate of ID 1

`Id` and `Type` parameters can also be given from the pipeline, likes following:  
    Get-JobTemplate -Id 1 | Find-Job

and also can omit `-Type` parameter:  
    Find-Job -Id 1

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

For more details, see [about_AWX.psm_Filter_parameter](about_AWX.psm_Filter_parameter.md).

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

### -Id
JobTemplate ID or it's object.
Filter to jobs in the JobTemplate for the specified ID.

> [!TIP]  
> Can specify `IResource` object.  
> For example: `-Id (Get-JobTemplate -Id 3)`, `-Id $jobTemplate`

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

### -LaunchType
Filter with `launch_type` field

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:
Accepted values: manual, relaunch, callback, scheduled, dependency, workflow, webhook, sync, scm

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
Filter by job name.
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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### AWX.Resources.IResource
JobTemplate ID or it's object.

## OUTPUTS

### AWX.Resources.JobTemplateJob
## NOTES

## RELATED LINKS

[Get-Job](Get-Job.md)

[Remove-Job](Remove-Job.md)

[Get-JobTemplate](Get-JobTemplate.md)

[Find-JobTemplate](Find-JobTemplate.md)

[Invoke-JobTemplate](Invoke-JobTemplate.md)

[Start-JobTemplate](Start-JobTemplate.md)

[Find-UnifiedJob](Find-UnifiedJob.md)
