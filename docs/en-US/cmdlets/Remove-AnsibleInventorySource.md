---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Remove-AnsibleInventorySource

## SYNOPSIS
Remove an InventorySource.

## SYNTAX

```
Remove-AnsibleInventorySource [-Id] <UInt64> [-Force] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Remove an InventorySource.

Implements following Rest API:  
- `/api/v2/inventory_sources/{id}/` (DELETE)

## EXAMPLES

### Example 1
```powershell
PS C:\> Remove-AnsibleInventorySource -Id 10
```

## PARAMETERS

### -Force
Don't confirm. (Ignore `-Confirm` and `-WhatIf`)

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
InventorySource ID to be removed.

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
InventorySource ID.

## OUTPUTS

### None
## NOTES

## RELATED LINKS

[Get-AnsibleInventorySource](Get-AnsibleInventorySource.md)

[Find-AnsibleInventorySource](Find-AnsibleInventorySource.md)

[New-AnsibleInventorySource](New-AnsibleInventorySource.md)

[Update-AnsibleInventorySource](Update-AnsibleInventorySource.md)
