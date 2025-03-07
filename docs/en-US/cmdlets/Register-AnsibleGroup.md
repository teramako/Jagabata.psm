---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Register-AnsibleGroup

## SYNOPSIS
Register a Group to another Group.

## SYNTAX

```
Register-AnsibleGroup [-Id] <UInt64> [-To] <UInt64> [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Register a Group to another Group.

Implements following Rest API:  
- `/api/v2/groups/{id}/children/` (POST)

## EXAMPLES

### Example 1
```powershell
PS C:\> Register-AnsibleGroup -Id 3 -To 1
```

Associate the Group of ID 3 to the Group of ID 1.

## PARAMETERS

### -Id
Group ID or its resource object to be registerted as a child.

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
Group ID or its resource object to be as a parent.

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
Group ID or its resource object to be registerted as a child.
See `-Id` parameter.

## OUTPUTS

### None

## NOTES

## RELATED LINKS

[Get-AnsibleGroup](Get-AnsibleGroup.md)

[Find-AnsibleGroup](Find-AnsibleGroup.md)

[New-AnsibleGroup](New-AnsibleGroup.md)

[Update-AnsibleGroup](Update-AnsibleGroup.md)

[Unregister-AnsibleGroup](Unregister-AnsibleGroup.md)

[Remove-AnsibleGroup](Remove-AnsibleGroup.md)
