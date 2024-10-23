---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Unregister-AnsibleHost

## SYNOPSIS
Remove a Host

## SYNTAX

```
Unregister-AnsibleHost [-Id] <UInt64> [-From] <UInt64> [-WhatIf]
 [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Remove a Host or disassociate from the group.

Implements following Rest API:  
- `/api/v2/groups/{id}/hosts/` (POST)

## EXAMPLES

### Example 1
```powershell
PS C:\> Unregister-AnsibleHost -Id 3 -From 1
```

Disassociate the Host of ID 3 from the Group ID 1.

## PARAMETERS

### -From
Parent Group ID.

> [!NOTE]  
> Can specify `IResource` object.  
> For example: `-From (Get-AnsibleGroup -Id 10)`, `-From @{ type="group"; id = 10 }`

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

[Register-AnsibleHost](Register-AnsibleHost.md)

[Remove-AnsibleHost](Remove-AnsibleHost.md)
