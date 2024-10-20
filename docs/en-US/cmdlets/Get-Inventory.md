---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-Inventory

## SYNOPSIS
Retrieve Inventories by the ID(s).

## SYNTAX

```
Get-Inventory [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve Inventories by the specified ID(s).

Implements following Rest API:  
- `/api/v2/inventories/{id}/`  

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-Inventory -Id 1
```

Retrieve an Inventory for Database ID 1.

## PARAMETERS

### -Id
List of database IDs for one or more Inventories.

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

### Jagabata.Resources.Inventory
## NOTES

## RELATED LINKS

[Find-Inventory](Find-Inventory.md)

[New-Inventory](New-Inventory.md)

[Update-Inventory](Update-Inventory.md)

[Remove-Inventory](Remove-Inventory.md)
