---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-AnsibleMetric

## SYNOPSIS
Retrieve Metrics.

## SYNTAX

```
Get-AnsibleMetric [<CommonParameters>]
```

## DESCRIPTION
Retrieve Metrics.

Implements following Rest API:  
- `/api/v2/metrics/`  

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-AnsibleMetric

Description                                                                                 Type SampleType                Value Labels
-----------                                                                                 ---- ----------                ----- ------
AWX System Information                                                                     gauge                               1 external_logger_enabled False…
Number of organizations                                                                    gauge                               2
Number of users                                                                            gauge                               3

(snip)

Number of active tasks in the worker pool when last task was submitted                     gauge                               1 node awx_1
Highest number of workers in worker pool in last collection interval, about 20s            gauge                               4 node awx_1
Fraction of time (in last collection interval) dispatcher was able to receive messages     gauge               0.997828014517918 node awx_1
```

## PARAMETERS

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### Jagabata.Cmdlets.MetricItem

## NOTES

## RELATED LINKS
