---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-JobStatistics

## SYNOPSIS
Retrieve statistics for job runs.

## SYNTAX

```
Get-JobStatistics [[-Period] <String>] [[-JobType] <String>] [<CommonParameters>]
```

## DESCRIPTION
Retrieve aggregate statistics about job runs suitable for graphing.

Implements following Rest API:  
- `/api/v2/dashboard/graphs/jobs/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-JobStatistics

Successful                     Failed
----------                     ------
Date       Count               Date       Count
----       -----               ----       -----
07/24/2024     0               07/24/2024     0
07/25/2024     0               07/25/2024     1
07/26/2024     0               07/26/2024     0
...
```

Retrieve statistics on all jobs for a month.

## PARAMETERS

### -JobType
Job type of statistics to be retrieved. (default: `all`)

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: all, inv_sync, playbook_run, scm_update

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Period
Period of statistics to be retrieved. (default: `month`)

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: month, two_weeks, week, day

Required: False
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None
## OUTPUTS

### Jagabata.Resources.JobStatistics
## NOTES

## RELATED LINKS
