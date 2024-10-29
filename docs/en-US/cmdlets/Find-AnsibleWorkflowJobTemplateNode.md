---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Find-AnsibleWorkflowJobTemplateNode

## SYNOPSIS
Retrieve WorkflowJobTemplateNodes.

## SYNTAX

### All (Default)
```
Find-AnsibleWorkflowJobTemplateNode [-OrderBy <String[]>] [-Search <String[]>] [-Filter <NameValueCollection>]
 [-Count <UInt16>] [-Page <UInt32>] [-All] [<CommonParameters>]
```

### WorkflowJobTemplate
```
Find-AnsibleWorkflowJobTemplateNode [-Template] <UInt64> [-OrderBy <String[]>] [-Search <String[]>]
 [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All] [<CommonParameters>]
```

### WorkflowJobTemplateNode
```
Find-AnsibleWorkflowJobTemplateNode [-Node] <UInt64> [-Linked] <WorkflowJobNodeLinkState> [-OrderBy <String[]>]
 [-Search <String[]>] [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All]
 [<CommonParameters>]
```

## DESCRIPTION
Retrieve the list of WorkflowJobTemplates.

Implementation of following API:  
- `/api/v2/workflow_job_template_nodes/`  
- `/api/v2/workflow_job_templates/{id}/workflow_nodes/`  
- `/api/v2/workflow_job_template_nodes/{id}/always_nodes/`  
- `/api/v2/workflow_job_template_nodes/{id}/success_nodes/`  
- `/api/v2/workflow_job_template_nodes/{id}/failure_nodes/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Find-AnsibleWorkflowJobTemplateNode
```

### Example 2
```powershell
PS C:\> Find-AnsibleWorkflowJobTemplateNode -Template 1
```

Retrieve WorkflowJobTemplateNodes associated with the WorkflowJobTemplate of ID 1

### Example 3
```powershell
PS C:\> Find-AnsibleWorkflowJobTemplateNode -Node 1 -For Always
```

Retrieve WorkflowJobTemplateNodes linked to always state of the WorkflowJobTemplateNode of ID 1.

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

### -Linked
Specifies at which state the WorkflowJobTemplateNode is linked.

```yaml
Type: WorkflowJobNodeLinkState
Parameter Sets: WorkflowJobTemplateNode
Aliases:
Accepted values: Always, Failure, Success

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Node
WorkflowJobTemplateNode ID or it's resource object.
Search for child nodes linked to that WorkflowJobTemplateNode.

> [!TIP]  
> Can specify an object which has `Type` and `Id`.  
> Example 1: `-Node (Get-AnsibleWorkflowJobTemplateNode -Id 10)`  
> Example 2: `-Node @{type="workflowjobtemplatenode"; id=10}`  
> Example 3: `-Node $nodes[0]`

```yaml
Type: UInt64
Parameter Sets: WorkflowJobTemplateNode
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

### -Template
WorkflowJobTemplate ID or it's resource object.
Find within the WorkflowJobTemplate.

> [!TIP]  
> Can specify an object which has `Type` and `Id`.  
> Example 1: `-Template (Get-AnsibleWorkflowJobTemplate -Id 3)`  
> Example 2: `-Template @{type="workflowjobtemplate"; id=3}`  
> Example 3: `-Template $wjt[0]`

```yaml
Type: UInt64
Parameter Sets: WorkflowJobTemplate
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.UInt64
WorkflowJobTemplate ID or WorkflowJobTemplateNode ID.
See `-Template` and `-Node` parameters.

## OUTPUTS

### Jagabata.Resources.WorkflowJobTemplateNode
## NOTES

## RELATED LINKS

[Get-AnsibleWorkflowJobTemplateNode](Get-AnsibleWorkflowJobTemplateNode.md)

[Get-AnsibleWorkflowJobTemplate](Get-AnsibleWorkflowJobTemplate.md)

[Find-AnsibleWorkflowJobTemplate](Find-AnsibleWorkflowJobTemplate.md)

[New-AnsibleWorkflowJobTemplateNode](New-AnsibleWorkflowJobTemplateNode.md)

[Update-AnsibleWorkflowJobTemplateNode](Update-AnsibleWorkflowJobTemplateNode.md)

[Remove-AnsibleWorkflowJobTemplateNode](Remove-AnsibleWorkflowJobTemplateNode.md)

[Register-AnsibleWorkflowJobTemplateNode](Register-AnsibleWorkflowJobTemplateNode.md)

[Unregister-AnsibleWorkflowJobTemplateNode](Unregister-AnsibleWorkflowJobTemplateNode.md)
