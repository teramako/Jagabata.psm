---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Find-WorkflowJob

## SYNOPSIS
Retrieve jobs for WorkflowJobTemplate.

## SYNTAX

### All (Default)
```
Find-WorkflowJob [-Name <String[]>] [-Status <String[]>] [-LaunchType <String[]>] [-OrderBy <String[]>]
 [-Search <String[]>] [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All]
 [<CommonParameters>]
```

### AssociatedWith
```
Find-WorkflowJob [-Type] <ResourceType> [-Id] <UInt64> [-Name <String[]>] [-Status <String[]>]
 [-LaunchType <String[]>] [-OrderBy <String[]>] [-Search <String[]>] [-Filter <NameValueCollection>]
 [-Count <UInt16>] [-Page <UInt32>] [-All] [<CommonParameters>]
```

### PipelineInput
```
Find-WorkflowJob [-Resource] <IResource> [-Name <String[]>] [-Status <String[]>] [-LaunchType <String[]>]
 [-OrderBy <String[]>] [-Search <String[]>] [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>]
 [-All] [<CommonParameters>]
```

## DESCRIPTION
Retrieve the list of jobs launched from WorkflowJobTemplates or sliced JobTemplates.

Implementation of following API:  
- `/api/v2/workflow_jobs/`  
- `/api/v2/workflow_job_templates/{id}/workflow_jobs/`  
- `/api/v2/job_templates/{id}/slice_workflow_jobs/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Find-WorkflowJob
```

### Example 2
```powershell
PS C:\> Find-WorkflowJob -Type WorkflowJobTemplate -Id 1
```

Retrieve jobs associated with the WorkflowJobTemplate of ID 1

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

### -Id
Datebase ID of the target resource.
Use in conjection with the `-Type` parameter.

```yaml
Type: UInt64
Parameter Sets: AssociatedWith
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
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
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Resource
Resource object to which the search target associated with.

The resource is accepted following types:  
- `WorkflowJobTemplate`  
- `JobTemplate`

```yaml
Type: IResource
Parameter Sets: PipelineInput
Aliases:

Required: True
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

### -Type
Resource type name of the target.
Use in conjection with the `-Id` parameter.

```yaml
Type: ResourceType
Parameter Sets: AssociatedWith
Aliases:
Accepted values: JobTemplate, WorkflowJobTemplate

Required: True
Position: 0
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

### Jagabata.Resources.WorkflowJob
## NOTES

## RELATED LINKS

[Get-WorkflowJob](Get-WorkflowJob.md)

[Remove-WorkflowJob](Remove-WorkflowJob.md)

[Get-WorkflowJobTemplate](Get-WorkflowJobTemplate.md)

[Find-WorkflowJobTemplate](Find-WorkflowJobTemplate.md)

[Invoke-WorkflowJobTemplate](Invoke-WorkflowJobTemplate.md)

[Start-WorkflowJobTemplate](Start-WorkflowJobTemplate.md)

[Find-UnifiedJob](Find-UnifiedJob.md)
