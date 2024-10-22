---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# New-Schedule

## SYNOPSIS
Create a Schedule.

## SYNTAX

```
New-Schedule -Name <String> [-Description <String>] -RRule <String> [-Disabled] -Template <IResource>
 [-ExtraData <String>] [-Inventory <UInt64>] [-ScmBranch <String>] [-JobType <JobType>] [-Tags <String>]
 [-SkipTags <String>] [-Limit <String>] [-DiffMode <Boolean>] [-Verbosity <JobVerbosity>] [-Forks <Int32>]
 [-ExecutionEnvironment <UInt64>] [-JobSliceCount <Int32>] [-Timeout <Int32>] [-WhatIf] [-Confirm]
 [<CommonParameters>]
```

## DESCRIPTION
Create a Schedule for the resource (`Project`, `InventorySource`, `JobTemplate`, `SystemJobTemplate` and `WorkflowJobTemplate`).

Implements following Rest API:  
- `/api/v2/projects/{id}/schedules/` (POST)  
- `/api/v2/inventory_sources/{id}/schedules/` (POST)  
- `/api/v2/job_templates/{id}/schedules/` (POST)  
- `/api/v2/system_job_templates/{id}/schedules/` (POST)  
- `/api/v2/workflow_job_templates/{id}/schedules/` (POST)

## EXAMPLES

### Example 1
```powershell
PS C:\> $jt = Get-JobTemplate -Id 10
PS C:\> $rrule = "DTSTART;TZID=Asia/Tokyo:20250310T213000 RRULE:INTERVAL=1;FREQ=YEARLY;BYMONTH=4;BYMONTHDAY=1"
PS C:\> New-Schedule -Name ScheduleName -RRrule $rrule -Template $jt
```

## PARAMETERS

### -Description
Optional description of the Schedule.

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

### -DiffMode
Turn Diff mode on or off.

```yaml
Type: Boolean
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Disabled
Create the schedule but as disabled.

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

### -ExecutionEnvironment
ExecutionEnvironment ID.

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

### -ExtraData
Specify extra variables.

Specify in JSON or YAML format.
You can also specify an object of type `IDictionary` as a parameter value.

Example: `-ExtraVars @{ key1 = "string"; key2 = 10; key3 = Get-Date }`

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

### -Forks
Number of forks.

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Inventory
Inventory ID.

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

### -JobSliceCount
Number of job slice count.

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -JobType
JobType ("Run" or "Check")

```yaml
Type: JobType
Parameter Sets: (All)
Aliases:
Accepted values: Run, Check

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Limit
Further limit selected hosts to an additional pattern.

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

### -Name
Name of the Schedule.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -RRule
Recurrence Rule.

See: [RFC5545 - 3.3.10. Recurrence Rule](https://datatracker.ietf.org/doc/html/rfc5545#section-3.3.10)

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ScmBranch
Branch to use in job run.

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

### -SkipTags
Skip tags. (commas `,` separated)

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

### -Tags
Job Tags. (commas `,` separated)

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

### -Template
Target resource for scheduled execution.

Available resource:
- `Project`  
- `InventorySource`  
- `JobTemplate`  
- `SystemJobTemplate`  
- `WorkflowJobTemplate`

```yaml
Type: IResource
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Timeout
Timeout value (seconds).

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Verbosity
Job verbosity.

```yaml
Type: JobVerbosity
Parameter Sets: (All)
Aliases:
Accepted values: Normal, Verbose, MoreVerbose, Debug, ConnectionDebug, WinRMDebug

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

### None
## OUTPUTS

### Jagabata.Resources.Schedule
New created Schedule object.

## NOTES

## RELATED LINKS

[Get-Schedule](Get-Schedule.md)

[Find-Schedule](Find-Schedule.md)

[Update-Schedule](Update-Schedule.md)

[Remove-Schedule](Remove-Schedule.md)
