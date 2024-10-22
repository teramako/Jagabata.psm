---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-Notification

## SYNOPSIS
Retrieve Notifications by the ID(s).

## SYNTAX

```
Get-Notification [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve Notifications by the specified ID(s).

Implements following Rest API:  
- `/api/v2/notifications/{id}/`  

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-Notification -Id 1 | Format-List

Id                   : 1
Type                 : Notification
Created              : 2024/07/18 16:41:05
Modified             : 2024/07/18 16:41:05
NotificationTemplate : 1
Name                 : TestNotification
Description          : Test
Status               : Successful
Error                :
NotificationsSent    : 1
NotificationType     : Slack
Recipients           : ['#proj-ansible']
Subject              : The approval node "Approval" was approved. http://*******
Body                 :
```

Retrieve a Notification for Database ID 1 and display as List format.

## PARAMETERS

### -Id
List of database IDs for one or more Notifications.

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

### Jagabata.Resources.Notification
## NOTES

## RELATED LINKS

[Find-Notification](Find-Notification.md)
