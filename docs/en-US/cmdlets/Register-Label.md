---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Register-Label

## SYNOPSIS
Register a Label to other resource.

## SYNTAX

```
Register-Label [-Id] <UInt64> [-To] <IResource> [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Register a Label to the target resource.

Implements following Rest API:  
- `/api/v2/inventories/{id}/labels/` (POST)  
- `/api/v2/job_templates/{id}/labels/` (POST)  
- `/api/v2/schedules/{id}/labels/` (POST)  
- `/api/v2/workflow_job_templates/{id}/labels/` (POST)  
- `/api/v2/workflow_job_template_nodes/{id}/labels/` (POST)

## EXAMPLES

### Example 1
```powershell
PS C:\> Register-Label -To (Get-Inventory -Id 2) -Id 1
```

Associate the Label of ID 1 to the Inventory of ID 2.

## PARAMETERS

### -Id
Label ID to be registered.

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
The resource to which registered.

Following resource is available:  
- `Inventory`  
- `JobTemplate`  
- `Schedule`  
- `WorkflowJobTemplate`  
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
Label ID.

## OUTPUTS

### None

## NOTES

## RELATED LINKS

[Get-Label](Get-Label.md)

[Find-Label](Find-Label.md)

[New-Label](New-Label.md)

[Unregister-Label](Unregister-Label.md)

[Update-Label](Update-Label.md)
