---
external help file: AWX.psm.dll-Help.xml
Module Name: AWX.psm
online version:
schema: 2.0.0
---

# Find-Label

## SYNOPSIS
Retrieve Labels.

## SYNTAX

### All (Default)
```
Find-Label [-OrderBy <String[]>] [-Search <String[]>] [-Filter <NameValueCollection>] [-Count <UInt16>]
 [-Page <UInt32>] [-All] [<CommonParameters>]
```

### AssociatedWith
```
Find-Label [-Type] <ResourceType> [-Id] <UInt64> [-OrderBy <String[]>] [-Search <String[]>]
 [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All] [<CommonParameters>]
```

### PipelineInput
```
Find-Label [-Resource] <IResource> [-OrderBy <String[]>] [-Search <String[]>] [-Filter <NameValueCollection>]
 [-Count <UInt16>] [-Page <UInt32>] [-All] [<CommonParameters>]
```

## DESCRIPTION
Retrieve the list of Labels.

Implementation of following API:  
- `/api/v2/labels/`  
- `/api/v2/inventories/{id}/labels/`  
- `/api/v2/job_templates/{id}/labels/`  
- `/api/v2/jobs/{id}/labels/`  
- `/api/v2/schedules/{id}/labels/`  
- `/api/v2/workflow_job_templates/{id}/labels/`  
- `/api/v2/workflow_jobs/{id}/labels/`  
- `/api/v2/workflow_job_template_nodes/{id}/labels/`  
- `/api/v2/workflow_job_nodes/{id}/labels/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Find-Label
```

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

### -OrderBy
Retrieve list in the specified orders.
Use `!` prefix to sort in reverse.
Multiple sorting fields are available by separating with a comma(`,`).

Default value: `id` (ascending order of ID)

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: ["id"]
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
Resource object to which the search target associated with.

The resource is accepted following types:  
- `Inventory`  
- `JobTemplate`  
- `Job`  
- `Schedule`  
- `WorkflowJobTemplate`  
- `WorkflowJob`  
- `WorkflowJobTemplateNode`  
- `WorkflowJobNode`

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

### -Type
Resource type name of the target.
Use in conjection with the `-Id` parameter.

```yaml
Type: ResourceType
Parameter Sets: AssociatedWith
Aliases:
Accepted values: Inventory, JobTemplate, Job, Schedule, WorkflowJobTemplate, WorkflowJob, WorkflowJobTemplateNode, WorkflowJobNode

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### AWX.Resources.IResource
Resource object to which the search target associated with.
See: `-Resource` parameter.

## OUTPUTS

### AWX.Resources.Label
## NOTES

## RELATED LINKS

[Get-Label](Get-Label.md)

[New-Label](New-Label.md)

[Register-Label](Register-Label.md)

[Unregister-Label](Unregister-Label.md)

[Update-Label](Update-Label.md)
