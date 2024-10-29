---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Find-AnsibleNotificationTemplateForApproval

## SYNOPSIS
Retrieve Approval NotificationTemplates.

## SYNTAX

### AssociatedWith (Default)
```
Find-AnsibleNotificationTemplateForApproval [-Type] <ResourceType> [-Id] <UInt64> [-OrderBy <String[]>]
 [-Search <String[]>] [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All]
 [<CommonParameters>]
```

### PipelineInput
```
Find-AnsibleNotificationTemplateForApproval [-Resource] <IResource> [-OrderBy <String[]>] [-Search <String[]>]
 [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All] [<CommonParameters>]
```

## DESCRIPTION
Retrieve the list of Approval NotificationTemplates enabled in the Organization or WorkflowJobTemplate.

Implementation of following API:  
- `/api/v2/organizations/{id}/notification_templates_approvals/`  
- `/api/v2/workflow_job_templates/{id}/notification_templates_approvals/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Find-AnsibleNotificationTemplateForApproval -Type Organization -Id 1
```

Retrieve Approval NotificationTemplates enabled in the Organization of ID 1.

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

### -Id
Datebase ID of the target resource.
Use in conjection with the `-Type` parameter.

```yaml
Type: UInt64
Parameter Sets: AssociatedWith
Aliases:

Required: True
Position: 1
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
Default value: ["id"]
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
- `Organization`  
- `WorkflowJobTemplate`

```yaml
Type: IResource
Parameter Sets: PipelineInput
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
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

### -Type
Resource type name of the target.
Use in conjection with the `-Id` parameter.

```yaml
Type: ResourceType
Parameter Sets: AssociatedWith
Aliases:
Accepted values: Organization, WorkflowJobTemplate

Required: True
Position: 0
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

### Jagabata.Resources.NotificationTemplate

## NOTES

## RELATED LINKS

[Get-AnsibleNotificationTemplate](Get-AnsibleNotificationTemplate.md)

[Find-AnsibleNotificationTemplate](Find-AnsibleNotificationTemplate.md)

[Find-AnsibleNotificationTemplateForStarted](Find-AnsibleNotificationTemplateForStarted.md)

[Find-AnsibleNotificationTemplateForError](Find-AnsibleNotificationTemplateForError.md)

[Find-AnsibleNotificationTemplateForSuccess](Find-AnsibleNotificationTemplateForSuccess.md)
