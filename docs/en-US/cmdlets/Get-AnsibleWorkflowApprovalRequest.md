---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-AnsibleWorkflowApprovalRequest

## SYNOPSIS
Retrieve request jobs for WorkflowApproval by ID(s).

## SYNTAX

```
Get-AnsibleWorkflowApprovalRequest [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve JobTemplates by the specified ID(s).

Implements following Rest API:  
- `/api/v2/workflow_approvals/{id}/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-AnsibleWorkflowApprovalRequest -Id 10

Id             Type Name            Status TimedOut Expire Remaining By       Finished            Elapsed LaunchedBy     WorkflowJob      WorkflowJobTemplate ApprovalTemplate
--             ---- ----            ------ -------- ------ --------- --       --------            ------- ----------     -----------      ------------------- ----------------
10 WorkflowApproval Sample-Approval Failed    False                  [1]admin 2024/07/25 15:46:16  54.573 [user][1]admin [20]ApprovedFlow [13]ApprovedFlow    [12]Sample-Approval
```

Retrieve a WorkflowApproval for Database ID 10.

## PARAMETERS

### -Id
List of database IDs for one or more WorkflowApproval.

```yaml
Type: UInt64[]
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.UInt64[]
One or more database IDs.

## OUTPUTS

### Jagabata.Resources.WorkflowApproval+Detail
## NOTES

## RELATED LINKS

[Find-AnsibleWorkflowApprovalRequest](Find-AnsibleWorkflowApprovalRequest.md)

[Approve-AnsibleWorkflowApprovalRequest](Approve-AnsibleWorkflowApprovalRequest.md)

[Deny-AnsibleWorkflowApprovalRequest](Deny-AnsibleWorkflowApprovalRequest.md)

[Remove-AnsibleWorkflowApprovalRequest](Remove-AnsibleWorkflowApprovalRequest.md)
