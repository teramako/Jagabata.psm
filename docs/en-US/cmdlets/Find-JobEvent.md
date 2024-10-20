---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Find-JobEvent

## SYNOPSIS
Retrieve Job Events.

## SYNTAX

### AssociatedWith
```
Find-JobEvent [-Type] <ResourceType> [-Id] <UInt64> [-AdHocCommandEvent] [-OrderBy <String[]>]
 [-Search <String[]>] [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All]
 [<CommonParameters>]
```

### PipelineInput
```
Find-JobEvent [-Resource] <IResource> [-AdHocCommandEvent] [-OrderBy <String[]>] [-Search <String[]>]
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
PS C:\> Find-JobEvent -Type Job -Id 10
```

Retrieve Events for JobTemplate job of ID 1

### Example 2
```powershell
PS C:\> Find-JobEvent -Type Host -Id 1 -AdHocCommandEvent
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

### -Type
Resource type name of the target.
Use in conjection with the `-Id` parameter.

```yaml
Type: ResourceType
Parameter Sets: AssociatedWith
Aliases:
Accepted values: Job, ProjectUpdate, InventoryUpdate, SystemJob, AdHocCommand, Host, Group

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

[Get-Job](Get-Job.md)

[Find-Job](Find-Job.md)

[Get-ProjectUpdateJob](Get-ProjectUpdateJob.md)

[Find-ProjectUpdateJob](Find-ProjectUpdateJob.md)

[Get-InventoryUpdateJob](Get-InventoryUpdateJob.md)

[Find-InventoryUpdateJob](Find-InventoryUpdateJob.md)

[Get-SystemJob](Get-SystemJob.md)

[Find-SystemJob](Find-SystemJob.md)

[Get-AdHocCommandJob](Get-AdHocCommandJob.md)

[Find-AdHocCommandJob](Find-AdHocCommandJob.md)
