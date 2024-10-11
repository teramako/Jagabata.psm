---
external help file: AWX.psm.dll-Help.xml
Module Name: AWX.psm
online version:
schema: 2.0.0
---

# Register-Credential

## SYNOPSIS
Register a Credential.

## SYNTAX

```
Register-Credential [-Id] <UInt64> [-To] <IResource> [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Register a Credential to the specified resource.

Implements following Rest API:  
- `/api/v2/organizations/{id}/galaxy_credentials/` (POST)  
- `/api/v2/inventory_sources/{id}/credentials/` (POST)  
- `/api/v2/job_templates/{id}/credentials/` (POST)  
- `/api/v2/schedules/{id}/credentials/` (POST)  
- `/api/v2/workflow_job_template_nodes/{id}/credentials/` (POST)

## EXAMPLES

### Example 1
```powershell
PS C:\> Register-Credential -Id 3 -To @{Type="JobTemplate"; Id=10}
```

## PARAMETERS

### -Id
Credential ID.

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

### -To
Target resource to be added to.

Following resource is available:  
- `Organization` (galaxy-token only)  
- `InventorySource`  
- `JobTemplate`  
- `Schedule`  
- `WorkflowJobTemplateNode`

```yaml
Type: IResource
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
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
Credential ID.

## OUTPUTS

### None

## NOTES

## RELATED LINKS

[Get-Credential](Get-Credential.md)

[Find-Credential](Find-Credential.md)

[New-Credential](New-Credential.md)

[Update-Credential](Update-Credential.md)

[Unregister-Credential](Unregister-Credential.md)

[Remove-Credential](Remove-Credential.md)
