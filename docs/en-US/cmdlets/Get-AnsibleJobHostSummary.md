---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-AnsibleJobHostSummary

## SYNOPSIS
Retrieve JobHostSummaries by ID(s).

## SYNTAX

```
Get-AnsibleJobHostSummary [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve JobHostSummaries by the specified ID(s).

Implements following Rest API:  
- `/api/v2/job_host_summaries/{id}/`  

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-AnsibleJobHostSummary -Id 1
```

Retrieve a JobHostSummary for Database ID 1.

## PARAMETERS

### -Id
List of database IDs for one or more JobHostSummary.

```yaml
Type: UInt64[]
Parameter Sets: (All)
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

### System.UInt64[]
List of database IDs for one or more JobHostSummary.

## OUTPUTS

### Jagabata.Resources.JobHostSummary
## NOTES

## RELATED LINKS

[Find-AnsibleJobHostSummary](Find-AnsibleJobHostSummary.md)
