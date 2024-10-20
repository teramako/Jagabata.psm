---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Remove-WorkflowApprovalRequest

## SYNOPSIS
Remove a WorkflowApproval.

## SYNTAX

```
Remove-WorkflowApprovalRequest [-Id] <UInt64> [-Force] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Remove a WorkflowApproval.

Implements following Rest API:  
- `/api/v2/workflow_approvals/{id}/` (DELETE)

## EXAMPLES

### Example 1
```powershell
PS C:\> Remove-WorkflowApprovalRequest -Id 30
```

## PARAMETERS

### -Force
Ignore confirmination.

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

### -Id
WorkflowApproval ID to be removed.

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

### -Confirm
Prompts you for confirmation before running the cmdlet.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: cf

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -WhatIf
Shows what would happen if the cmdlet runs.
The cmdlet is not run.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: wi

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
WorkflowApproval ID.

## OUTPUTS

### None
## NOTES

## RELATED LINKS

[Get-WorkflowApprovalRequest](Get-WorkflowApprovalRequest.md)

[Find-WorkflowApprovalRequest](Find-WorkflowApprovalRequest.md)

[Approve-WorkflowApprovalRequest](Approve-WorkflowApprovalRequest.md)

[Deny-WorkflowApprovalRequest](Deny-WorkflowApprovalRequest.md)
