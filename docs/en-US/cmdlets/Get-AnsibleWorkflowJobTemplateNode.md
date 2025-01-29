---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-AnsibleWorkflowJobTemplateNode

## SYNOPSIS
Retrieve nodes for WorkflowJobTemplate by ID(s).

## SYNTAX

```
Get-AnsibleWorkflowJobTemplateNode [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve nodes for WorkflowJobTemplate by specified ID(s).

Implementation of following API:  
- `/api/v2/workflow_job_template_nodes/{id}/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-AnsibleWorkflowJobTemplateNode -Id 1

Id                    Type WorkflowJobTemplate Template ID Template Type Template Name SuccessNodes FailureNodes AlwaysNodes
--                    ---- ------------------- ----------- ------------- ------------- ------------ ------------ -----------
 1 WorkflowJobTemplateNode [13]TestWorkflow              9           Job Test_1        {}           {}           {4}
```

Retrieve a node in WorkflowJob for Database ID 1.

## PARAMETERS

### -Id
List of database IDs for one or more nodes in WorkflowJobTemplate.

```yaml
Type: UInt64[]
Parameter Sets: (All)
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

### System.UInt64[]
One or more database IDs.

## OUTPUTS

### Jagabata.Resources.WorkflowJobTemplateNode
## NOTES

## RELATED LINKS

[Find-AnsibleWorkflowJobTemplateNode](Find-AnsibleWorkflowJobTemplateNode.md)

[New-AnsibleWorkflowJobTemplateNode](New-AnsibleWorkflowJobTemplateNode.md)

[Update-AnsibleWorkflowJobTemplateNode](Update-AnsibleWorkflowJobTemplateNode.md)

[Remove-AnsibleWorkflowJobTemplateNode](Remove-AnsibleWorkflowJobTemplateNode.md)

[Register-AnsibleWorkflowJobTemplateNode](Register-AnsibleWorkflowJobTemplateNode.md)

[Unregister-AnsibleWorkflowJobTemplateNode](Unregister-AnsibleWorkflowJobTemplateNode.md)
