---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Update-AnsibleNotificationTemplate

## SYNOPSIS
Update a NotificationTemplate.

## SYNTAX

```
Update-AnsibleNotificationTemplate [-Id] <UInt64> [-Name <String>] [-Description <String>]
 [-Organization <UInt64>] [-Type <NotificationType>] [-Configuration <IDictionary>] [-Messages <IDictionary>]
 [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Update a NotificationTemplate.

Implements following Rest API:  
- `/api/v2/notification_templates/{id}/` (PATCH)

## EXAMPLES

### Example 1
```powershell
PS C:\> Update-AnsibleNotificationTemplate -Id 2 -Name "Renamed"
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

### -Id
NotificationTemplate ID or its resource object to be updated.

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

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Organization
Organization ID or its resource object.

```yaml
Type: UInt64
Parameter Sets: (All)
Aliases:

Required: False
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

Required: False
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

### System.UInt64
NotificationTemplate ID or its resource object to be updated.
See `-Id` parameter.

## OUTPUTS

### Jagabata.Resources.NotificationTemplate
Updated NotificationTemplate object.

## NOTES

## RELATED LINKS

[Get-AnsibleNotificationTemplate](Get-AnsibleNotificationTemplate.md)

[Find-AnsibleNotificationTemplate](Find-AnsibleNotificationTemplate.md)

[New-AnsibleNotificationTemplate](New-AnsibleNotificationTemplate.md)

[Enable-AnsibleNotificationTemplate](Enable-AnsibleNotificationTemplate.md)

[Disable-AnsibleNotificationTemplate](Diable-NotificationTemplate.md)

[Remove-AnsibleNotificationTemplate](Remove-AnsibleNotificationTemplate.md)
