---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Show-AnsibleSchedule

## SYNOPSIS
Preview scheduled datetimes

## SYNTAX

### RRule
```
Show-AnsibleSchedule [-RRule] <String> [<CommonParameters>]
```

### Schedule
```
Show-AnsibleSchedule [-Schedule] <Schedule> [<CommonParameters>]
```

## DESCRIPTION
Preview scheduled datetimes. (Max 10)

Implements following Rest API:  
- `/api/v2/schedules/preview/` (POST)

## EXAMPLES

### Example 1
```powershell
PS C:\> Show-AnsibleSchedule -RRule "DTSTART;TZID=Asia/Tokyo:20250101T000000 RRULE:INTERVAL=1;FREQ=MONTHLY;BYMONTHDAY=1,-1;BYHOUR=9"

Local                                    Utc
-----                                    ---
2025-01-01T09:00:00+09:00                2025-01-01T00:00:00Z
2025-01-31T09:00:00+09:00                2025-01-31T00:00:00Z
2025-02-01T09:00:00+09:00                2025-02-01T00:00:00Z
2025-02-28T09:00:00+09:00                2025-02-28T00:00:00Z
2025-03-01T09:00:00+09:00                2025-03-01T00:00:00Z
2025-03-31T09:00:00+09:00                2025-03-31T00:00:00Z
2025-04-01T09:00:00+09:00                2025-04-01T00:00:00Z
2025-04-30T09:00:00+09:00                2025-04-30T00:00:00Z
2025-05-01T09:00:00+09:00                2025-05-01T00:00:00Z
2025-05-31T09:00:00+09:00                2025-05-31T00:00:00Z
```

### Example 2
```powershell
PS C:\> Get-AnsibleSchedule -Id 3 | Show-AnsibleSchedule
```

## PARAMETERS

### -RRule
Scheduling data which has Recurrence Rule a.k.a `RRULE` and starting datetime a.k.a `DTSTART`.

```yaml
Type: String
Parameter Sets: RRule
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Schedule
Schedule object which is retrieved with `Get-AnsibleSchedule` or `Find-AnsibleSchedule`.

```yaml
Type: Schedule
Parameter Sets: Schedule
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String
Scheduling data which has Recurrence Rule a.k.a `RRULE` and starting datetime a.k.a `DTSTART`.

### Jagabata.Resources.Schedule
Schedule object which is retrieved with `Get-AnsibleSchedule` or `Find-AnsibleSchedule`.

## OUTPUTS

### Jagabata.Resources.SchedulePreview
An objects with up to 10 most recent scheduled datetimes, each with a local date and UTC date

## NOTES

## RELATED LINKS

[Get-AnsibleSchedule](Get-AnsibleSchedule.md)

[Find-AnsibleSchedule](Find-AnsibleSchedule.md)
