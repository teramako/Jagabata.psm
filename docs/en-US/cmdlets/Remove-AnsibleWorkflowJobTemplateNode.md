---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Remove-AnsibleWorkflowJobTemplateNode

## SYNOPSIS
Remove a WorkflowJobTemplateNode.

## SYNTAX

```
Remove-AnsibleWorkflowJobTemplateNode [-Id] <UInt64> [-Force] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Remove a WorkflowJobTemplateNode.

Implements following Rest API:  
- `/api/v2/workflow_job_template_nodes/{id}/` (DELETE)

## EXAMPLES

### Example 1
```powershell
PS C:\> Remove-AnsibleWorkflowJobTemplateNode -Id 30
```

## PARAMETERS

### -Force
Don't confirm. (Ignore `-Confirm` and `-WhatIf`)

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
WorkflowJobTemplateNode ID to be removed.

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
WorkflowJobTemplateNode ID.

## OUTPUTS

### None
## NOTES

## RELATED LINKS

[Get-AnsibleWorkflowJobTemplateNode](Get-AnsibleWorkflowJobTemplateNode.md)

[Find-AnsibleWorkflowJobTemplateNode](Find-AnsibleWorkflowJobTemplateNode.md)

[New-AnsibleWorkflowJobTemplateNode](New-AnsibleWorkflowJobTemplateNode.md)

[Update-AnsibleWorkflowJobTemplateNode](Update-AnsibleWorkflowJobTemplateNode.md)

[Register-AnsibleWorkflowJobTemplateNode](Register-AnsibleWorkflowJobTemplateNode.md)

[Unregister-AnsibleWorkflowJobTemplateNode](Unregister-AnsibleWorkflowJobTemplateNode.md)
