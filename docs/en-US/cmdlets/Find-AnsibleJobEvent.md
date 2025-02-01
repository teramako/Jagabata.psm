---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Find-AnsibleJobEvent

## SYNOPSIS
Retrieve Job Events.

## SYNTAX

```
Find-AnsibleJobEvent [-Resource] <IResource> [-AdHocCommandEvent] [-OrderBy <String[]>] [-Search <String[]>]
 [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All] [<CommonParameters>]
```

## DESCRIPTION
Retrieve the list of Events for Job, ProjectUpdate, InventoryUpdate, SystemJob and AdHocCommand.

Implementation of following API:  
- `/api/v2/jobs/{id}/job_events/`  
- `/api/v2/ad_hoc_commands/{id}/events/`  
- `/api/v2/system_jobs/{id}/events/`  
- `/api/v2/project_updates/{id}/events/`  
- `/api/v2/inventory_updates/{id}/events/`  
- `/api/v2/groups/{id}/job_events/`  
- `/api/v2/hosts/{id}/ad_hoc_command_events/`  
- `/api/v2/hosts/{id}/job_events/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Find-AnsibleJobEvent -Resource Job:10
```

Retrieve Events for JobTemplate job of ID 1

### Example 2
```powershell
PS C:\> Find-AnsibleJobEvent -Resource Host:1 -AdHocCommandEvent
```

Retrieve AdHocCommand (not JobTemplate job) Events associated with Host of ID 1.

## PARAMETERS

### -AdHocCommandEvent
Retrieve AdHocCommand Events instead of JobTemplate's Events.
Only affected for a **Host** Type

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

Default value: `counter` (ascending order of `counter`)

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: ["counter"]
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
Target resource object from which to retrieve JobEvent.

A resource is an object with `Id` and `Type` properties.
And `Type` should be following value:  
- `Job`             : JobTempalte's job  
- `ProjectUpdate`   : Project's update job  
- `InventoryUpdate` : InventorySource's update job  
- `AdHocCommand`    : AdHocCommand's job  
- `SystemJob`       : SystemJobTemplate's job  
- `Host`            : Host (Retrieve events for jobs or ad-hoc-command jobs run on the host.)  
- `Group`           : Group (Retrieve events for jobs run on hosts belonging to the group.)

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
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Search
Search words. (case-insensitive)

Target fields: `stdout`

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
The object has `Id` and `Type` properties.

And `Type` should be following value:  
- `Job`  
- `ProjectUpdate`  
- `InventoryUpdate`  
- `SystemJob`  
- `AdHocCommand`  
- `Host`  
- `Group`

## OUTPUTS

### Jagabata.Resources.IJobEventBase
JobEvent objects that extend `IJobEventBase` interface.  
- Job             : `Jagabata.Resources.JobEvent`  
- ProjectUpdate   : `Jagabata.Resources.ProjectUpdateJobEvent`  
- InventoryUpdate : `Jagabata.Resources.InventoryUpdateJobEvent`  
- SystemJob       : `Jagabata.Resources.SystemJobEvent`  
- AdHocCommand    : `Jagabata.Resources.AdHocCommandJobEvent`

## NOTES

## RELATED LINKS

[Get-AnsibleJob](Get-AnsibleJob.md)

[Find-AnsibleJob](Find-AnsibleJob.md)

[Get-AnsibleProjectUpdateJob](Get-AnsibleProjectUpdateJob.md)

[Find-AnsibleProjectUpdateJob](Find-AnsibleProjectUpdateJob.md)

[Get-AnsibleInventoryUpdateJob](Get-AnsibleInventoryUpdateJob.md)

[Find-AnsibleInventoryUpdateJob](Find-AnsibleInventoryUpdateJob.md)

[Get-AnsibleSystemJob](Get-AnsibleSystemJob.md)

[Find-AnsibleSystemJob](Find-AnsibleSystemJob.md)

[Get-AnsibleAdHocCommandJob](Get-AnsibleAdHocCommandJob.md)

[Find-AnsibleAdHocCommandJob](Find-AnsibleAdHocCommandJob.md)
