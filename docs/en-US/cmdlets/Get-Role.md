---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-Role

## SYNOPSIS
Retrieve Roles by the ID(s).

## SYNTAX

```
Get-Role [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve Roles by the specified ID(s).

Implements following Rest API:  
- `/api/v2/roles/{id}/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-Role -Id 1

Id Type Name                 Description                          ResourceId ResourceType ResourceName
-- ---- ----                 -----------                          ---------- ------------ ------------
 1 Role System Administrator Can manage all aspects of the system
```

Retrieve a Role for Database ID 1.

## PARAMETERS

### -Id
List of database IDs for one or more Roles.

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

### Jagabata.Resources.Role
## NOTES

## RELATED LINKS

[Find-Role](Find-Role.md)

[Grant-Role](Grant-Role.md)

[Revoke-Role](Revoke-Role.md)
