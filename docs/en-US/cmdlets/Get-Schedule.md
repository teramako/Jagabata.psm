---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-Schedule

## SYNOPSIS
Retrieve Schedules by the ID(s).

## SYNTAX

```
Get-Schedule [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve Schedules by the specified ID(s).

Implements following Rest API:  
- `/api/v2/schedules/{id}/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-Schedule -Id 1 | Format-List

Id                     : 1
Type                   : Schedule
Created                : 2023/11/04 16:19:08
Modified               : 2023/11/04 16:19:08
Name                   : Cleanup Job Schedule
Description            : Automatically Generated Schedule
Enabled                : True
Rrule                  : DTSTART:20231104T071908Z RRULE:FREQ=WEEKLY;INTERVAL=1;BYDAY=SU
Dtstart                :
Dtend                  :
NextRun                : 2024/08/11 16:19:08
Timezone               :
Until                  :
UnifiedJobTemplate     : 1
UnifiedJobTemplateName : Cleanup Job Details
UnifiedJobType         : SystemJob
ExtraData              : {[days, 120]}
Inventory              :
ScmBranch              :
JobType                :
JobTags                :
SkipTags               :
Limit                  :
DiffMode               :
Verbosity              :
ExecutionEnvironment   :
Forks                  :
JobSliceCount          :
Timeout                :
```

Retrieve a Schedule for Database ID 1 and display as List format.

## PARAMETERS

### -Id
List of database IDs for one or more Schedules.

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

### Jagabata.Resources.Schedule
## NOTES

## RELATED LINKS

[Find-Schedule](Find-Schedule.md)

[New-Schedule](New-Schedule.md)

[Update-Schedule](Update-Schedule.md)

[Remove-Schedule](Remove-Schedule.md)
