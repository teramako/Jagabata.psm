---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-AnsibleNotificationTemplate

## SYNOPSIS
Retrieve NotificationTemplates by the ID(s).

## SYNTAX

```
Get-AnsibleNotificationTemplate [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve NotificationTemplates by the specified ID(s).

Implements following Rest API:  
- `/api/v2/notification_templates/{id}/`  

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-AnsibleNotificationTemplate 1 | Format-List

Id                        : 1
Type                      : NotificationTemplate
Created                   : 2024/06/08 22:38:16
Modified                  : 2024/06/08 23:40:23
Name                      : TestNotification
Description               : Test
Organization              : 2
OrganizationName          : TestOrg
NotificationType          : Slack
NotificationConfiguration : {[token, $encrypted$], [channels, ["#proj-ansible"]], [hex_color, ]}
StartMessage              : NMessage { Body = , Message =  }
SuccessMessage            : NMessage { Body = , Message =  }
ErrorMessage              : NMessage { Body = , Message =  }
ApprovedMessage           : NMessage { Body = , Message =  }
DeniedMessage             : NMessage { Body = , Message =  }
RunningMessage            : NMessage { Body = , Message =  }
```

Retrieve a NotificationTemplate for Database ID 1 and display as List format.

## PARAMETERS

### -Id
List of database IDs for one or more NotificationTemplates.

```yaml
Type: UInt64[]
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

### System.UInt64[]
One or more database IDs.

## OUTPUTS

### Jagabata.Resources.NotificationTemplate
## NOTES

## RELATED LINKS

[Find-AnsibleNotificationTemplate](Find-AnsibleNotificationTemplate.md)

[New-AnsibleNotificationTemplate](New-AnsibleNotificationTemplate.md)

[Enable-AnsibleNotificationTemplate](Enable-AnsibleNotificationTemplate.md)

[Disable-AnsibleNotificationTemplate](Diable-NotificationTemplate.md)

[Update-AnsibleNotificationTemplate](Update-AnsibleNotificationTemplate.md)

[Remove-AnsibleNotificationTemplate](Remove-AnsibleNotificationTemplate.md)
