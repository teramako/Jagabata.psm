---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Update-AnsibleSchedule

## SYNOPSIS
Update a Schedule.

## SYNTAX

```
Update-AnsibleSchedule [-Id] <UInt64> [-Name <String>] [-Description <String>] [-RRule <String>]
 [-Enable <Boolean>] [-ExtraData <String>] [-Inventory <UInt64>] [-ScmBranch <String>] [-JobType <JobType>]
 [-Tags <String>] [-SkipTags <String>] [-Limit <String>] [-DiffMode <Boolean>] [-Verbosity <JobVerbosity>]
 [-Forks <Int32>] [-ExecutionEnvironment <UInt64>] [-JobSliceCount <Int32>] [-Timeout <Int32>] [-WhatIf]
 [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Update a Schedule.

Implements following Rest API:  
- `/api/v2/schedules/{id}/` (PATCH)

## EXAMPLES

### Example 1
```powershell
PS C:\> Update-AnsibleSchedule -Id 10 -Enable $true
```

### Example 2
```powershell
PS C:\> $calendar = [Jagabata.Schedule.Calendar]::new([datetime]"2025-03-10 21:30")
PS C:\> $rule1 = ([Jagabata.Schedule.RRule]@{ Freq = "Yearly"; }).SetMonth(4).SetMonthDay(1)
PS C:\> $calendar.RRules.Add($rule1)
PS C:\> $calendar.ToString();
DTSTART;TZID=Asia/Tokyo:20250310T213000 RRULE:FREQ=YEARLY;INTERVAL=1;BYMONTHDAY=1;BYMONTH=4
PS C:\> Update-AnsibleSchedule -Id 10 -RRrule $calendar
```

Update scheduling for ID 10.

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

### -Enable
Turn the Schedule enabled or disabled.

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

### -Id
Schedule ID to be updated.

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
Name of the schedule.

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

### -RRule
Scheduling data which has Recurrence Rule a.k.a `RRULE` and starting datetime a.k.a `DTSTART`.

See: [RFC5545 - 3.3.10. Recurrence Rule](https://datatracker.ietf.org/doc/html/rfc5545#section-3.3.10)

> [!TIP]
> Scheduling data can be made from `Jagabata.Schedule.Calendar` and `Jagabata.Schedule.RRule`.
> 
> ```powershell
> $calendar = [Jagabata.Schedule.Calendar]::new([datetime]"2025-03-10 21:30") # DTSTART
> $rule1 = ([Jagabata.Schedule.RRule]@{ Freq = "Yearly"; }).SetMonth(4).SetMonthDay(1) # April 1 of each year
> $rule2 = ([Jagabata.Schedule.RRule]@{ Freq = "Monthly"; Interval = 3 }).SetWeekDay("SA").SetPos(1) # First Saturday of every three months
> $calendar.RRules.Add($rule1)
> $calendar.RRules.Add($rule2)  
> ```

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

### System.UInt64
## OUTPUTS

### Jagabata.Resources.Schedule
Updated Schedule object.

## NOTES

## RELATED LINKS

[Get-AnsibleSchedule](Get-AnsibleSchedule.md)

[Find-AnsibleSchedule](Find-AnsibleSchedule.md)

[New-AnsibleSchedule](New-AnsibleSchedule.md)

[Remove-AnsibleSchedule](Remove-AnsibleSchedule.md)

[Show-AnsibleSchedule](Show-AnsibleSchedule.md)
