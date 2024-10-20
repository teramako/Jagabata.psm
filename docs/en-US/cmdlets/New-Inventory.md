---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# New-Inventory

## SYNOPSIS
Create an Inventory.

## SYNTAX

### NormalInventory (Default)
```
New-Inventory [-Organization] <UInt64> [-Name] <String> [-Description <String>] [-Variables <String>]
 [-PreventInstanceGroupFallback] [-WhatIf] [-Confirm] [<CommonParameters>]
```

### SmartInventory
```
New-Inventory [-Organization] <UInt64> [-Name] <String> [-Description <String>] [-Variables <String>]
 [-PreventInstanceGroupFallback] [-AsSmartInventory] -HostFilter <String> [-WhatIf] [-Confirm]
 [<CommonParameters>]
```

## DESCRIPTION
Create an Inventory.

Implements following Rest API:  
- `/api/v2/inventories/` (POST)

## EXAMPLES

### Example 1
```powershell
PS C:\> New-Inventory -Organization 1 -Name test_inventory
```

Create a new Inventory into the Organization of ID 1.

## PARAMETERS

### -AsSmartInventory
Create as Smart Inventory

```yaml
Type: SwitchParameter
Parameter Sets: SmartInventory
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Description
Optional description of the Inventory.

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

### -HostFilter
Filter that will be applied to the hosts of the Inventory.

```yaml
Type: String
Parameter Sets: SmartInventory
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
Name of the Inventory.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
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

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PreventInstanceGroupFallback
If enabled, the inventory will prevent adding any organization instance groups to the list of preferred instances groups to run associated job templates on.
If this setting is enabled and you provided an empty list, the global instance groups will be applied.

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

### -Variables
Specify in JSON or YAML format.
You can also specify an object of type `IDictionary` as a parameter value.

Example: `-Variables @{ key1 = "value 1"; key2 = 10; }`

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

### None
## OUTPUTS

### Jagabata.Resources.Inventory
New created Inventory object.

## NOTES

## RELATED LINKS

[Get-Inventory](Get-Inventory.md)

[Find-Inventory](Find-Inventory.md)

[Update-Inventory](Update-Inventory.md)

[Remove-Inventory](Remove-Inventory.md)
