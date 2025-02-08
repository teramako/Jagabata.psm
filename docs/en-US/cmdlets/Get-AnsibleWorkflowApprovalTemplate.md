---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-AnsibleWorkflowApprovalTemplate

## SYNOPSIS
Retrieve WorkflowApprovalTemplates by the ID(s).

## SYNTAX

```
Get-AnsibleWorkflowApprovalTemplate [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve WorkflowApprovalTemplates by the specified ID(s).

Implements following Rest API:  
- `/api/v2/workflow_approval_templates/{id}/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-AnsibleWorkflowApprovalTemplate -Id 12

Id                     Type Name            Description   Status Modified            LastJobRun          NextJobRun Options Note
--                     ---- ----            -----------   ------ --------            ----------          ---------- ------- ----
12 WorkflowApprovalTemplate Sample-Approval Before Launch Failed 2024/07/25 14:44:57 2024/07/25 15:46:16                    {[Timeout, 0], [WorkflowTemplate, [20]ApprovedFlow]}
```

Retrieve a JobTemplate for Database ID 12.

## PARAMETERS

### -Id
List of database IDs for one or more WorkflowApprovalTemplates.

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
List of database IDs for one or more WorkflowApprovalTemplates.

## OUTPUTS

### Jagabata.Resources.WorkflowApprovalTemplate
## NOTES

To get the ID number of the WorkflowApprovalTemplate, see `Template ID` column in the nodes from `Find-AnsibleWorkflowJobTemplateNode` command or `ApprovalTemplate` column in the jobs from `Find-AnsibleWorkflowApprovalRequest` command.

## RELATED LINKS

[Get-AnsibleWorkflowJobTemplateNode](Get-AnsibleWorkflowJobTemplateNode.md)

[Find-AnsibleWorkflowJobTemplateNode](Find-AnsibleWorkflowJobTemplateNode.md)

[New-AnsibleWorkflowJobTemplateNode](New-AnsibleWorkflowJobTemplateNode.md)
