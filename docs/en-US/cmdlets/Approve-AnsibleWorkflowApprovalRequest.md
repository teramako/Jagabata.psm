---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Approve-AnsibleWorkflowApprovalRequest

## SYNOPSIS
Approve requests for WorkflowApproval.

## SYNTAX

```
Approve-AnsibleWorkflowApprovalRequest [-Id] <UInt64> [<CommonParameters>]
```

## DESCRIPTION
Approve requests for pending WorkflowApprovals.
And output the results for jobs of those.

Implements following Rest API:  
- `/api/v2/workflow_approvals/{id}/approve/`  

## EXAMPLES

### Example 1
```powershell
PS C:\> Approve-AnsibleWorkflowApprovalRequest -Id 1
```

Approve the WorkflowApproval of ID 1.

### Example 2
```powershell
PS C:\> Find-AnsibleWorkflowApprovalRequest -Status pending | Approve-AnsibleWorkflowApprovalRequest
```

Approve all pending WorkflowApprovals.

## PARAMETERS

### -Id
Database ID for the WorkflowApproval.

```yaml
Type: UInt64
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

### Jagabata.Resources.ResourceType
Input by `Type` property in the pipeline object.

Acceptable values: `WorkflowApproval` (only)

### System.UInt64
Input by `Id` property in the pipeline object.

Database ID for `WorkflowApproval`

## OUTPUTS

### Jagabata.Resources.WorkflowApproval
## NOTES

## RELATED LINKS

[Deny-AnsibleWorkflowApprovalRequest](./Deny-AnsibleWorkflowApprovalRequest.md)

[Find-AnsibleWorkflowApprovalRequest](./Find-AnsibleWorkflowApprovalRequest.md)
