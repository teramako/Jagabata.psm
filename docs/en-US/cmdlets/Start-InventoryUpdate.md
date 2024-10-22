---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Start-InventoryUpdate

## SYNOPSIS
Invoke (update) InventorySources.

## SYNTAX

### Id
```
Start-InventoryUpdate [-Id] <UInt64> [<CommonParameters>]
```

### CheckId
```
Start-InventoryUpdate [-Id] <UInt64> [-Check] [<CommonParameters>]
```

### Resource
```
Start-InventoryUpdate [-Source] <IResource> [<CommonParameters>]
```

### CheckResource
```
Start-InventoryUpdate [-Source] <IResource> [-Check] [<CommonParameters>]
```

## DESCRIPTION
Update InventorySources.
Multiple InventorySources in the Inventory may be udpated, when an Inventory is specified bye `-Inventory` parameter.

This command only sends a request to update InventorySources, not wait for the job is completed.
So, the returned job object will be non-completed status.
Use `Wait-UnifiedJob` command to wait for the job to complete later.

Implementation of following API:  
- `/api/v2/inventory_sources/{id}/update/`  
- `/api/v2/inventries/{id}/update_inventory_sources/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Start-InventoryUpdate -Id 11

 Id            Type Name                        JobType LaunchType  Status Finished Elapsed LaunchedBy     Template        Note
 --            ---- ----                        ------- ----------  ------ -------- ------- ----------     --------        ----
130 InventoryUpdate TestInventory - test_source             Manual Pending                0 [user][1]admin [11]test_source {[Inventory, [2]TestInventory], [Source, Scm], [SourcePath, inventory/hosts.ini]}
```

Update an InventorySource ID 11.

## PARAMETERS

### -Check
Check wheter InventorySource(s) can be updated.

```yaml
Type: SwitchParameter
Parameter Sets: CheckId, CheckResource
Aliases:

Required: True
Position: Named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
InventorySource ID to be updated.

```yaml
Type: UInt64
Parameter Sets: Id, CheckId
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Source
A `Inventory` or `InventorySource` object.

If the value is `Inventory`, all of InventorySources in the Inventory will be updated or checked.

```yaml
Type: IResource
Parameter Sets: Resource, CheckResource
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.UInt64
InventorySource ID

### Jagabata.Resources.InventorySource
InventorySource object

### Jagabata.Resources.Inventory
Inventory object containing the InventorySources to be updated.

## OUTPUTS

### Jagabata.Resources.InventoryUpdateJob+Detail
InventoryUpdate job object (non-completed status)

### System.Management.Automation.PSObject
Results of checked wheter the InventorySources can be updated.

## NOTES

## RELATED LINKS

[Invoke-InventoryUpdate](Invoke-InventoryUpdate)

[Get-InventoryUpdateJob](Get-InventoryUpdateJob.md)

[Find-InventoryUpdateJob](Find-InventoryUpdateJob.md)

[Wait-UnifiedJob](Wait-UnifiedJob.md)
