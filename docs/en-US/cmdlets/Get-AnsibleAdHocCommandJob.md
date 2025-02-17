---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-AnsibleAdHocCommandJob

## SYNOPSIS
Retrieve AdHocCommand job details by ID(s).

## SYNTAX

```
Get-AnsibleAdHocCommandJob [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve AdHocCommand jobs by the specified ID(s).

Implements following Rest API:  
- `/api/v2/ad_hoc_commands/{id}/`  

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-AnsibleAdHocCommandJob -Id 10
```

Retrieve an ActivityStream for Database ID 1.

### Example 2
```powershell
PS C:\> 10..15 | Get-AnsibleAdHocCommandJob
```

Retrieve AdHocCommand jobs with ID numbers 10 though 15.

## PARAMETERS

### -Id
List of database IDs for one or more AdHocCommand jobs.

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
List of database IDs for one or more AdHocCommand jobs.

## OUTPUTS

### Jagabata.Resources.AdHocCommand+Detail
## NOTES

## RELATED LINKS

[Find-AnsibleAdHocCommandJob](Find-AnsibleAdHocCommandJob.md)

[Remove-AnsibleAdHocCommandJob](Remove-AnsibleAdHocCommandJob.md)
