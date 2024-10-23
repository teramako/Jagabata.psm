---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Register-AnsibleHost

## SYNOPSIS
Register a Host to a Group.

## SYNTAX

```
Register-AnsibleHost [-Id] <UInt64> [-To] <UInt64> [-WhatIf] [-Confirm]
 [<CommonParameters>]
```

## DESCRIPTION
Register a User to a Group.

Implements following Rest API:  
- `/api/v2/groups/{id}/hosts/` (POST)

## EXAMPLES

### Example 1
```powershell
PS C:\> Register-AnsibleHost -Id 3 -To 1
```

Associate the Host of ID 3 to the Group of ID 1.

## PARAMETERS

### -Id
Host Id.

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

### -To
Parent Group Id.

> [!NOTE]  
> Can specify `IResource` object.  
> For example: `-To (Get-AnsibleGroup -Id 10)`, `-To @{ type="group"; id = 10 }`

```yaml
Type: UInt64
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

### System.UInt64
Host Id.

## OUTPUTS

### None

## NOTES

## RELATED LINKS

[Get-AnsibleHost](Get-AnsibleHost.md)

[Find-AnsibleHost](Find-AnsibleHost.md)

[New-AnsibleHost](New-AnsibleHost.md)

[Update-AnsibleHost](Update-AnsibleHost.md)

[Unregister-AnsibleHost](Unregister-AnsibleHost.md)

[Remove-AnsibleHost](Remove-AnsibleHost.md)
