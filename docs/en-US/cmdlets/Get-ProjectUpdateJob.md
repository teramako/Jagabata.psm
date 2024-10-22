---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-ProjectUpdateJob

## SYNOPSIS
Retrieve ProjectUpdate job details by ID(s).

## SYNTAX

```
Get-ProjectUpdateJob [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve ProjectUpdate job details by the specified ID(s).

Implements following Rest API:  
- `/api/v2/project_updates/{id}/`  

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-ProjectUpdateJob -Id 10

Id          Type Name         JobType LaunchType     Status Finished           Elapsed LaunchedBy               Template             Note
--          ---- ----         ------- ----------     ------ --------           ------- ----------               --------             ----
10 ProjectUpdate Demo Project     Run       Sync Successful 2024/07/11 9:27:49    1.39 [project][6]Demo Project [6][git]Demo Project {[Branch, ], [Revision, ****], [Url, https://***]}
```

Retrieve a ProjectUpdate job for Database ID 10.

## PARAMETERS

### -Id
List of database IDs for one or more ProjectUpdate jobs.

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

### Jagabata.Resources.ProjectUpdateJob+Detail
## NOTES

## RELATED LINKS

[Find-ProjectUpdateJob](Find-ProjectUpdateJob.md)

[Remove-ProjectUpdateJob](Remove-ProjectUpdateJob.md)
