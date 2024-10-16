---
external help file: AWX.psm.dll-Help.xml
Module Name: AWX.psm
online version:
schema: 2.0.0
---

# Find-WorkflowJobNode

## SYNOPSIS
Retrieve nodes for WorkflowJob.

## SYNTAX

### All (Default)
```
Find-WorkflowJobNode [-OrderBy <String[]>] [-Search <String[]>] [-Filter <NameValueCollection>]
 [-Count <UInt16>] [-Page <UInt32>] [-All] [<CommonParameters>]
```

### WorkflowJob
```
Find-WorkflowJobNode [-Job] <UInt64> [-OrderBy <String[]>] [-Search <String[]>] [-Filter <NameValueCollection>]
 [-Count <UInt16>] [-Page <UInt32>] [-All] [<CommonParameters>]
```

### WorkflowJobNode
```
Find-WorkflowJobNode [-Node] <UInt64> [-Linked] <WorkflowLinkState> [-OrderBy <String[]>] [-Search <String[]>]
 [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All] [<CommonParameters>]
```

## DESCRIPTION
Retrieve the list of WorkflowJobNodes.

Implementation of following API:  
- `/api/v2/workflow_job_nodes/`  
- `/api/v2/workflow_jobs/{id}/workflow_nodes/`  
- `/api/v2/workflow_job_nodes/{id}/always_nodes/`  
- `/api/v2/workflow_job_nodes/{id}/success_nodes/`  
- `/api/v2/workflow_job_nodes/{id}/failure_nodes/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Find-WorkflowJobNode
```

### Example 2
```powershell
PS C:\> Find-WorkflowJobNode -Job 10
```

Retrieve nodes associated with the WorkflowJob of ID 10

### Example 3
```powershell
PS C:\> Find-WorkflowJobNode -Node 1 -LinkType Always
```

Retrieve WorkflowJobNodes linked to always state of the WorkflowJobNode of ID 1.

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

### -Job
WorkflowJob ID or it's resource object.
Find within the WorkflowJob.

> [!TIP]  
> Can specify an object which has `Type` and `Id`.  
> Example 1: `-Job (Get-WorkflowJob -Id 3)`  
> Example 2: `-Job @{type="workflowjob"; id=3}`  
> Example 3: `-Job $workflowJobs[0]`

```yaml
Type: UInt64
Parameter Sets: WorkflowJob
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Linked
Specifies at which state the WorkflowJobNode is linked.

```yaml
Type: WorkflowLinkState
Parameter Sets: WorkflowJobNode
Aliases:
Accepted values: Always, Failure, Success

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Node
WorkflowJobNode ID or it's resource object.
Search for child nodes linked to that WorkflowJobNode.

> [!TIP]  
> Can specify an object which has `Type` and `Id`.  
> Example 1: `-Node (Get-WorkflowJobNode -Id 10)`  
> Example 2: `-Node @{type="workflowjobnode"; id=10}`  
> Example 3: `-Node $nodes[0]`

```yaml
Type: UInt64
Parameter Sets: WorkflowJobNode
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
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

Target fields: `unified_job_template__name`, `unified_job_template__description`

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

### System.UInt64
WorkflowJob ID or WorkflowJobNode ID.
See `-Job` and `-Node` parameters.

## OUTPUTS

### AWX.Resources.WorkflowJobNode
## NOTES

## RELATED LINKS

[Get-WorkflowJobNode](Get-WorkflowJobNode.md)

[Get-WorkflowJob](Get-WorkflowJob.md)

[Find-WorkflowJob](Find-WorkflowJob.md)
