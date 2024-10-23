---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-AnsibleInventoryUpdateJob

## SYNOPSIS
Retrieve InventoryUpdate job details by ID(s).

## SYNTAX

```
Get-AnsibleInventoryUpdateJob [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve InventoryUpdate job details by the specified ID(s).

Implements following Rest API:  
- `/api/v2/inventory_updates/{id}/`  

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-AnsibleInventoryUpdateJob -Id 1
```

Retrieve an InventoryUpdate job for Database ID 1.

## PARAMETERS

### -Id
List of database IDs for one or more InventoryUpdate jobs.

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

### Jagabata.Resources.InventoryUpdateJob
## NOTES

## RELATED LINKS

[Find-AnsibleInventoryUpdateJob](Find-AnsibleInventoryUpdateJob.md)

[Remove-AnsibleInventoryUpdateJob](Remove-AnsibleInventoryUpdateJob.md)
