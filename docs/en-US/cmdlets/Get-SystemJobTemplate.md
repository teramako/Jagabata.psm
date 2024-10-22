---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-SystemJobTemplate

## SYNOPSIS
Retrieve SystemJobTemplates by the ID(s).

## SYNTAX

```
Get-SystemJobTemplate [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve SystemJobTemplates by the specified ID(s).

Implements following Rest API:  
- `/api/v2/system_job_template/{id}/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-SystemJobTemplate -Id 1

Id              Type Name                Description            Status Modified            LastJobRun          NextJobRun          Options Note
--              ---- ----                -----------            ------ --------            ----------          ----------          ------- ----
 1 SystemJobTemplate Cleanup Job Details Remove job history Successful 2023/11/04 16:19:08 2024/07/28 16:19:34 2024/08/11 16:19:08         {[JobType, cleanup_jobs]}
```

Retrieve a SystemJobTemplate for Database ID 1.

## PARAMETERS

### -Id
List of database IDs for one or more SystemJobTemplates.

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

### Jagabata.Resources.SystemJobTemplate
## NOTES

## RELATED LINKS

[Find-SystemJobTemplate](Find-SystemJobTemplate.md)
