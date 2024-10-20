---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# New-Team

## SYNOPSIS
Create a Team.

## SYNTAX

```
New-Team -Organization <UInt64> -Name <String> [-Description <String>] [-WhatIf] [-Confirm]
 [<CommonParameters>]
```

## DESCRIPTION
Create a Team.

Implements following Rest API:  
- `/api/v2/teams/` (POST)

## EXAMPLES

### Example 1
```powershell
PS C:\> New-Team -Organization 2 -Name DemoTeam1 -Description "でもチーム"

Id Type Name      Description Created             Modified            OrganizationName
-- ---- ----      ----------- -------             --------            ----------------
 4 Team DemoTeam1 でもチーム  09/11/2024 13:24:09 09/11/2024 13:24:09 TestOrg
```

## PARAMETERS

### -Description
Optional description of the Team.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: ""
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
Team name.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Organization
Organization ID.

```yaml
Type: UInt64
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Confirm
Prompts you for confirmation before running the cmdlet.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: cf

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -WhatIf
Shows what would happen if the cmdlet runs.
The cmdlet is not run.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: wi

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None
## OUTPUTS

### Jagabata.Resources.Team
New created Team object.

## NOTES

## RELATED LINKS

[Get-Team](Get-Team.md)

[Find-Team](Find-Team.md)

[Remove-Team](Remove-Team.md)

[Update-Team](Update-Team.md)
