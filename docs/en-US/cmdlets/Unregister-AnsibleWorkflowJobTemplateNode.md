---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Unregister-AnsibleWorkflowJobTemplateNode

## SYNOPSIS
Unlink WorkflowJobTemplateNode from the parent.

## SYNTAX

```
Unregister-AnsibleWorkflowJobTemplateNode [-Id] <UInt64> -From <UInt64> [-WhatIf] [-Confirm]
 [<CommonParameters>]
```

## DESCRIPTION
Unlink WorkflowJobTemplateNode from the parent.

Implements following Rest API:  
- `/api/v2/workflow_job_template_nodes/{id}/success_nodes/` (POST)  
- `/api/v2/workflow_job_template_nodes/{id}/failure_nodes/` (POST)  
- `/api/v2/workflow_job_template_nodes/{id}/always_nodes/` (POST)

## EXAMPLES

### Example 1
```powershell
PS C:\> Unregister-AnsibleWorkflowJobTemplateNode -Id 2 -From 1
```

## PARAMETERS

### -From
WorkflowJobTemplateNode ID or its resource object to be parent.

```yaml
Type: UInt64
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
WorkflowJobTemplateNode ID or its resource object to be registered as a child.

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
WorkflowJobTemplateNode ID or its resource object to registered as a child.
See `-Id` parameter.

## OUTPUTS

### None
## NOTES

## RELATED LINKS

[Get-AnsibleWorkflowJobTemplateNode](Get-AnsibleWorkflowJobTemplateNode.md)

[Find-AnsibleWorkflowJobTemplateNode](Find-AnsibleWorkflowJobTemplateNode.md)

[New-AnsibleWorkflowJobTemplateNode](New-AnsibleWorkflowJobTemplateNode.md)

[Update-AnsibleWorkflowJobTemplateNode](Update-AnsibleWorkflowJobTemplateNode.md)

[Remove-AnsibleWorkflowJobTemplateNode](Remove-AnsibleWorkflowJobTemplateNode.md)

[Register-AnsibleWorkflowJobTemplateNode](Register-AnsibleWorkflowJobTemplateNode.md)
