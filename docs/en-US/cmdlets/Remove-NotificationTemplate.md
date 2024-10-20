---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Remove-NotificationTemplate

## SYNOPSIS
Remove a NotificationTemplate.

## SYNTAX

```
Remove-NotificationTemplate [-Id] <UInt64> [-Force] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Remove a NotificationTemplate.

Implements following Rest API:  
- `/api/v2/notification_templates/{id}/` (DELETE)

## EXAMPLES

### Example 1
```powershell
PS C:\> Remove-NotificationTemplate -Id 4
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
NotificationTemplate ID to be removed.

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
NotificationTemplate ID.

## OUTPUTS

### None
## NOTES

## RELATED LINKS

[Get-NotificationTemplate](Get-NotificationTemplate.md)

[Find-NotificationTemplate](Find-NotificationTemplate.md)

[New-NotificationTemplate](New-NotificationTemplate.md)

[Enable-NotificationTemplate](Enable-NotificationTemplate.md)

[Disable-NotificationTemplate](Diable-NotificationTemplate.md)

[Update-NotificationTemplate](Update-NotificationTemplate.md)
