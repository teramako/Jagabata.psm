---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# New-NotificationTemplate

## SYNOPSIS
Create a NotificationTemplate.

## SYNTAX

```
New-NotificationTemplate [-Name] <String> [-Description <String>] -Organization <UInt64>
 -Type <NotificationType> [-Configuration <IDictionary>] [-Messages <IDictionary>] [-WhatIf] [-Confirm]
 [<CommonParameters>]
```

## DESCRIPTION
Create a NotificationTemplate.

Implements following Rest API:  
- `/api/v2/notification_templates/` (POST)

## EXAMPLES

### Example 1
```powershell
PS C:\> New-NotificationTemplate -Name NotifySlack -Organization 1 -Type Slack -Configuration @{ token = "tokenA"; channles = @("#channel") }
```

## PARAMETERS

### -Configuration
Value corresponding to NotificationType.

```yaml
Type: IDictionary
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Description
Optional description of the NotificationTemplate.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Messages
Optional custom messages.

```yaml
Type: IDictionary
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
Name of the NotificationTemplate.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Organization
Organization ID.

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

### -Type
Type of Notification

```yaml
Type: NotificationType
Parameter Sets: (All)
Aliases:
Accepted values: Email, Grafana, IRC, Mattemost, Pagerduty, RoketChat, Slack, Twillo, Webhook

Required: True
Position: Named
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

### None
## OUTPUTS

### Jagabata.Resources.NotificationTemplate
New created NotificationType object.

## NOTES

## RELATED LINKS

[Get-NotificationTemplate](Get-NotificationTemplate.md)

[Find-NotificationTemplate](Find-NotificationTemplate.md)

[Enable-NotificationTemplate](Enable-NotificationTemplate.md)

[Disable-NotificationTemplate](Diable-NotificationTemplate.md)

[Update-NotificationTemplate](Update-NotificationTemplate.md)

[Remove-NotificationTemplate](Remove-NotificationTemplate.md)
