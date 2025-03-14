---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Find-AnsibleWorkflowJobNode

## SYNOPSIS
Retrieve nodes for WorkflowJob.

## SYNTAX

### All (Default)
```
Find-AnsibleWorkflowJobNode [-OrderBy <String[]>] [-Search <String[]>] [-Filter <NameValueCollection>]
 [-Count <UInt16>] [-Page <UInt32>] [-All] [<CommonParameters>]
```

### WorkflowJob
```
Find-AnsibleWorkflowJobNode [-Job] <UInt64> [-OrderBy <String[]>] [-Search <String[]>]
 [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All] [<CommonParameters>]
```

### WorkflowJobNode
```
Find-AnsibleWorkflowJobNode [-Node] <UInt64> [-Linked] <WorkflowJobNodeLinkState> [-OrderBy <String[]>]
 [-Search <String[]>] [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All]
 [<CommonParameters>]
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
PS C:\> Find-AnsibleWorkflowJobNode
```

### Example 2
```powershell
PS C:\> Find-AnsibleWorkflowJobNode -Job 10
```

Retrieve nodes associated with the WorkflowJob of ID 10

### Example 3
```powershell
PS C:\> Find-AnsibleWorkflowJobNode -Node 1 -LinkType Always
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

### -Job
WorkflowJob ID or it's resource object.
Find within the WorkflowJob.

> [!TIP]  
> Can specify an object which has `Type` and `Id`.  
> Example 1: `-Job (Get-AnsibleWorkflowJob -Id 3)`  
> Example 2: `-Job @{type="workflowjob"; id=3}`  
> Example 3: `-Job $workflowJobs[0]`

> [!TIP]  
> Can specify the resource as string like `WorkflowJob:1` (Format: `{Type}:{Id}`).
> And also accept objects have `type` and `id` properties.  
>
> For example:  
>  - `-Job (Get-AnsibleWorkflowJob -Id 1)`  
>  - `-Job @{ type = "workflowjob"; id = 1 }`  
>  - `-Job workflowjob:1`

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
Type: WorkflowJobNodeLinkState
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
> Can specify the resource as string like `WorkflowJobNode:1` (Format: `{Type}:{Id}`).
> And also accept objects have `type` and `id` properties.  
>
> For example:  
>  - `-Node (Get-AnsibleWorkflowJobNode -Id 1)`  
>  - `-Node @{ type = "workflowjobnode"; id = 1 }`  
>  - `-Node workflowJobNode:1`

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

### Jagabata.Resources.WorkflowJobNode
## NOTES

## RELATED LINKS

[Get-AnsibleWorkflowJobNode](Get-AnsibleWorkflowJobNode.md)

[Get-AnsibleWorkflowJob](Get-AnsibleWorkflowJob.md)

[Find-AnsibleWorkflowJob](Find-AnsibleWorkflowJob.md)
