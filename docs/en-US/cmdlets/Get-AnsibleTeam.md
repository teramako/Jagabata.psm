---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-AnsibleTeam

## SYNOPSIS
Retrieve Teams by the ID(s).

## SYNTAX

```
Get-AnsibleTeam [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve Teams by the specified ID(s).

Implements following Rest API:  
- `/api/v2/teams/{id}/`  

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-AnsibleTeam -Id 1

Id Type Name      Description Created             Modified            OrganizationName
-- ---- ----      ----------- -------             --------            ----------------
 1 Team TestTeam1 Sample Team 2024/06/03 18:17:55 2024/06/03 18:17:55 SampleOrg
```

Retrieve a Team for Database ID 1.

## PARAMETERS

### -Id
List of database IDs for one or more JobTemplates.

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

### Jagabata.Resources.Team
## NOTES

## RELATED LINKS

[Find-AnsibleTeam](Find-AnsibleTeam.md)

[New-AnsibleTeam](New-AnsibleTeam.md)

[Update-AnsibleTeam](Update-AnsibleTeam.md)

[Remove-AnsibleTeam](Remove-AnsibleTeam.md)
