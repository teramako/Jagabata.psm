---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Grant-Role

## SYNOPSIS
Grant Roles.

## SYNTAX

```
Grant-Role [-Roles] <IResource[]> [-To] <IResource> [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Grant Roles to a User or Team.

Implements following Rest API:  
- `/api/v2/users/{id}/roles/` (POST)  
- `/api/v2/teams/{id}/roles/` (POST)

## EXAMPLES

### Example 1
```powershell
PS C:\> $roles = Find-ObjectRole -Type JobTemplate -Id 10
PS C:\> $user = Get-User -Id 2
PS C:\> Grant-Role -Roles $roles -To $user
```

Grant all roles of JobTemplate ID 10 to the Uesr of ID 2.

## PARAMETERS

### -Roles
Target role objects to be granted.

```yaml
Type: IResource[]
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -To
Target resource (`User` or `Team`) to be granted to.

```yaml
Type: IResource
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
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

### Jagabata.Resources.IResource[]
Role objects to be granted.

## OUTPUTS

### None
## NOTES

## RELATED LINKS

[Get-Role](Get-Role.md)

[Find-Role](Find-Role.md)

[Find-ObjectRole](Find-ObjectRole.md)

[Revoke-Role](Revoke-Roke.md)
