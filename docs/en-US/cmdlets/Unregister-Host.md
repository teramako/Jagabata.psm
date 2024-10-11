---
external help file: AWX.psm.dll-Help.xml
Module Name: AWX.psm
online version:
schema: 2.0.0
---

# Unregister-Host

## SYNOPSIS
Remove a Host

## SYNTAX

```
Unregister-Host [-Id] <UInt64> [-From] <UInt64> [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Remove a Host or disassociate from the group.

Implements following Rest API:  
- `/api/v2/groups/{id}/hosts/` (POST)

## EXAMPLES

### Example 1
```powershell
PS C:\> Unregister-Host -Id 3 -From 1
```

Disassociate the Host of ID 3 from the Group ID 1.

## PARAMETERS

### -From
Parent Group ID.

> [!NOTE]  
> Can specify `IResource` object.  
> For example: `-From (Get-Group -Id 10)`, `-From @{ type="group"; id = 10 }`

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

[Get-Host](Get-Host.md)

[Find-Host](Find-Host.md)

[New-Host](New-Host.md)

[Update-Host](Update-Host.md)

[Register-Host](Register-Host.md)

[Remove-Host](Remove-Host.md)
