---
external help file: AWX.psm.dll-Help.xml
Module Name: AWX.psm
online version:
schema: 2.0.0
---

# Register-Group

## SYNOPSIS
Register a Group to another Group.

## SYNTAX

```
Register-Group [-Id] <UInt64> [-To] <UInt64> [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Register a Group to another Group.

Implements following Rest API:  
- `/api/v2/groups/{id}/children/` (POST)

## EXAMPLES

### Example 1
```powershell
PS C:\> Register-Group -Id 3 -ToGroup 1
```

Associate the Group of ID 3 to the Group of ID 1.

## PARAMETERS

### -Id
Group ID to be a child.

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
Group ID to be the parent.

> [!NOTE]  
> Can specify `IResource` object.  
> For example: `-To (Get-Group -Id 10)`, `-To @{ type="group"; id = 10 }`

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
Group Id to be a child.

## OUTPUTS

### None

## NOTES

## RELATED LINKS

[Get-Group](Get-Group.md)

[Find-Group](Find-Group.md)

[New-Group](New-Group.md)

[Update-Group](Update-Group.md)

[Unregister-Group](Unregister-Group.md)

[Remove-Group](Remove-Group.md)
