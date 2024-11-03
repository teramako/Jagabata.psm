---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Deny-AnsibleWorkflowApprovalRequest

## SYNOPSIS
Deny requests for WorkflowApproval.

## SYNTAX

```
Deny-AnsibleWorkflowApprovalRequest [-Id] <UInt64> [<CommonParameters>]
```

## DESCRIPTION
Deny requests for pending WorkflowApprovals.
And output the results for jobs of those.

Implements following Rest API:  
- `/api/v2/workflow_approvals/{id}/deny/`  

## EXAMPLES

### Example 1
```powershell
PS C:\> Deny-AnsibleWorkflowApprovalRequest -Id 1
```

Deny the WorkflowApproval of ID 1.

### Example 2
```powershell
PS C:\> Find-AnsibleWorkflowApprovalRequest -Status pending | Deny-AnsibleWorkflowApprovalRequest
```

Deny all pending WorkflowApprovals.

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
Accept pipeline input: True (ByValue)
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

[Approve-AnsibleWorkflowApprovalRequest](./Approve-AnsibleWorkflowApprovalRequest.md)

[Find-AnsibleWorkflowApprovalRequest](./Find-AnsibleWorkflowApprovalRequest.md)
