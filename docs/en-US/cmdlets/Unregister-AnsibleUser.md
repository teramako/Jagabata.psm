---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Unregister-AnsibleUser

## SYNOPSIS
Unregister a User from other resource.

## SYNTAX

```
Unregister-AnsibleUser [-Id] <UInt64> [-From] <IResource> [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Unregister a Label from the target resource.

Implements following Rest API:  
- `/api/v2/organization/{id}/users/` (POST)  
- `/api/v2/teams/{id}/users/` (POST)  
- `/api/v2/roles/{id}/users/` (POST)

## EXAMPLES

### Example 1
```powershell
PS C:\> Remove-AnsibleUser -Id 2 -From (Get-AnsibleOrganization -Id 1)
```

Disassociate the User of ID 2 from the Organization of ID 1.

## PARAMETERS

### -From
Parent resource object to be unregistered from.

Following resource is available:  
- `Organization`  
- `Team`  
- `Role`

> [!TIP]  
> Can specify the resource as string like `Team:1` (Format: `{Type}:{Id}`).
> And also accept objects have `type` and `id` properties.  
>
> For example:  
>  - `-From (Get-AnsibleTeam -Id 1)`  
>  - `-From @{ type = "team"; id = 1 }`  
>  - `-From team:1`

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

### -Id
User ID or its resource object to be unregistered.

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
User ID or its resource object to be unregistered.

## OUTPUTS

### None

## NOTES

## RELATED LINKS

[Get-AnsibleUser](Get-AnsibleUser.md)

[Find-AnsibleUser](Find-AnsibleUser.md)

[New-AnsibleUser](New-AnsibleUser.md)

[Update-AnsibleUser](Update-AnsibleUser.md)

[Register-AnsibleUser](Register-AnsibleUser.md)

[Unregister-AnsibleUser](Unregister-AnsibleUser.md)

[Remove-AnsibleUser](Remove-AnsibleUser.md)
