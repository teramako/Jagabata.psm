---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Find-AnsibleNotification

## SYNOPSIS
Retrieve Notifications.

## SYNTAX

```
Find-AnsibleNotification [[-Resource] <IResource>] [-OrderBy <String[]>] [-Search <String[]>]
 [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All] [<CommonParameters>]
```

## DESCRIPTION
Retrieve the list of Notifications.

Implementation of following API:  
- `/api/v2/notifications/`  
- `/api/v2/notification_templates/{id}/notifications/`  
- `/api/v2/jobs/{id}/notifications/`  
- `/api/v2/workflow_jobs/{id}/notifications/`  
- `/api/v2/system_jobs/{id}/notifications/`  
- `/api/v2/project_updates/{id}/notifications/`  
- `/api/v2/inventory_updates/{id}/job_templates/`  
- `/api/v2/ad_hoc_commands/{id}/job_templates/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Find-AnsibleNotification
```

### Example 2
```powershell
PS C:\> Find-AnsibleNotification -Resource NotificationTemplate:1
```

Retrieve Notifications associated with the NotificationTemplate of ID 1

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

Default value: `!id` (descending order of ID)

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: ["!id"]
Accept pipeline input: False
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
Default value: 1
Accept pipeline input: False
Accept wildcard characters: False
```

### -Resource
Resource object to which the search target associated with.

The resource is accepted following types:  
- `NotificationTemplate`  
- `Job`  
- `WorkflowJob`  
- `SystemJob`  
- `ProjectUpdate`  
- `InventoryUpdate`  
- `AdHocCommand`

> [!TIP]  
> Can specify the resource as string like `Job:1` (Format: `{Type}:{Id}`).
> And also accept objects have `type` and `id` properties.  
>
> For example:  
>  - `-Resource (Get-AnsibleJob -Id 1)`  
>  - `-Resource @{ type = "job"; id = 1 }`  
>  - `-Resource job:1`

```yaml
Type: IResource
Parameter Sets: (All)
Aliases: associatedWith, r

Required: False
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Search
Search words. (case-insensitive)

Target fields: `subject`, `notification_type`, `body`

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
Resource object to which the search target associated with.
See: `-Resource` parameter.

## OUTPUTS

### Jagabata.Resources.Notification
## NOTES

## RELATED LINKS

[Get-AnsibleNotification](Get-AnsibleNotification.md)

[Get-AnsibleNotificationTemplate](Get-AnsibleNotificationTemplate.md)

[Find-AnsibleNotificationTemplate](Find-AnsibleNotificationTemplate.md)
