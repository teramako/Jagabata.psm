---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Update-Team

## SYNOPSIS
Update a Team.

## SYNTAX

```
Update-Team [-Id] <UInt64> [-Name <String>] [-Description <String>] [-Organization <UInt64>] [-WhatIf]
 [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Update a Team. 

Implements following Rest API:  
- `/api/v2/teams/{id}/` (PATCH)

## EXAMPLES

### Example 1
```powershell
PS C:\> Update-Team -Id 2 -Name "NewName"

Id Type Name    Description Created             Modified            OrganizationName
-- ---- ----    ----------- -------             --------            ----------------
 2 Team NewName デモ 1      09/11/2024 15:27:45 09/11/2024 15:28:19 TestOrg
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

### -Id
Team ID to be updated.

```yaml
Type: UInt64
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Name
Team name.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
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

Required: False
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

### System.UInt64
Team ID.

## OUTPUTS

### Jagabata.Resources.Team
Updated Team object.

## NOTES

## RELATED LINKS

[Get-Team](Get-Team.md)

[Find-Team](Find-Team.md)

[New-Team](New-Team.md)

[Remove-Team](Remove-Team.md)
