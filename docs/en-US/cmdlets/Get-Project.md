---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-Project

## SYNOPSIS
Retrieve Projects by the ID(s).

## SYNTAX

```
Get-Project [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve Projects by the specified ID(s).

Implements following Rest API:  
- `/api/v2/projects/{id}/`  

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-Project -Id 6

Id    Type Name         Description     Status Modified            LastJobRun         NextJobRun Options Note
--    ---- ----         -----------     ------ --------            ----------         ---------- ------- ----
 6 Project Demo Project             Successful 2023/11/04 16:20:27 2024/07/02 0:01:13            None    {[Scm, [git]https://github.com/ansible/ansible-tower-samples], [Branch, ]}
```

Retrieve a Project for Database ID 6.

## PARAMETERS

### -Id
List of database IDs for one or more Projects.

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

### Jagabata.Resources.Project
## NOTES

## RELATED LINKS

[Find-Project](Find-Project.md)

[New-Project](New-Project.md)

[Update-Project](Update-Project.md)

[Remove-Project](Remove-Project.md)
