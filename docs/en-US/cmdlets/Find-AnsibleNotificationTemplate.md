---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Find-AnsibleNotificationTemplate

## SYNOPSIS
Retrieve NotificationTemplates.

## SYNTAX

```
Find-AnsibleNotificationTemplate [[-Organization] <UInt64>] [-OrderBy <String[]>] [-Search <String[]>]
 [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All] [<CommonParameters>]
```

## DESCRIPTION
Retrieve the list of NotificationTemplates.

Implementation of following API:  
- `/api/v2/notification_templates/`  
- `/api/v2/organizations/{id}/notifications/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Find-AnsibleNotificationTemplate
```

### Example 2
```powershell
PS C:\> Find-AnsibleNotificationTemplate -Organization 1
```

Retrieve NotificationTemplates associated with the Organization of ID 1.

## PARAMETERS

### -All
Retrieve resources from all pages.

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

### -Count
Number to retrieve per page.

```yaml
Type: UInt16
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: 20
Accept pipeline input: False
Accept wildcard characters: False
```

### -Filter
Filtering various fields.

For examples:  
- `name__icontains=test`: "name" field contains "test" (case-insensitive).  
- `"name_ in=test,demo", created _gt=2024-01-01`: "name" field is "test" or "demo" and created after 2024-01-01.  
- `@{ Name = "name"; Value = "test"; Type = "Contains"; Not = $true }`: "name" field NOT contains "test"

For more details, see [about_Jagabata.psm_Filter_parameter](about_Jagabata.psm_Filter_parameter.md).

```yaml
Type: NameValueCollection
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -OrderBy
Retrieve list in the specified orders.
Use `!` prefix to sort in reverse.
Multiple sorting fields are available by separating with a comma(`,`).

Default value: `id` (ascending order of ID)

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Organization
Organization ID or it's object.
Filter to NotoficationTemplates in the Organization for the specified ID.

```yaml
Type: UInt64
Parameter Sets: (All)
Aliases:

Required: False
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Page
Page number.

```yaml
Type: UInt32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Search
Search words. (case-insensitive)

Target fields: `name`, `description`

Multiple words are available by separating with a comma(`,`).

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### Jagabata.Resources.IResource
Organization ID or it's object.

## OUTPUTS

### Jagabata.Resources.NotificationTemplate
## NOTES

## RELATED LINKS

[Get-AnsibleNotificationTemplate](Get-AnsibleNotificationTemplate.md)

[Find-AnsibleNotificationTemplateForApproval](Find-AnsibleNotificationTemplateForApproval.md)

[Find-AnsibleNotificationTemplateForStarted](Find-AnsibleNotificationTemplateForStarted.md)

[Find-AnsibleNotificationTemplateForError](Find-AnsibleNotificationTemplateForError.md)

[Find-AnsibleNotificationTemplateForSuccess](Find-AnsibleNotificationTemplateForSuccess.md)

[Get-AnsibleNotification](Get-AnsibleNotification.md)

[Find-AnsibleNotification](Find-AnsibleNotification.md)

[New-AnsibleNotificationTemplate](New-AnsibleNotificationTemplate.md)

[Enable-AnsibleNotificationTemplate](Enable-AnsibleNotificationTemplate.md)

[Disable-AnsibleNotificationTemplate](Diable-NotificationTemplate.md)

[Update-AnsibleNotificationTemplate](Update-AnsibleNotificationTemplate.md)

[Remove-AnsibleNotificationTemplate](Remove-AnsibleNotificationTemplate.md)
