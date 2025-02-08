---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Start-AnsibleInventoryUpdate

## SYNOPSIS
Invoke (update) InventorySources.

## SYNTAX

### Launch (Default)
```
Start-AnsibleInventoryUpdate [-Source] <IResource> [<CommonParameters>]
```

### Check
```
Start-AnsibleInventoryUpdate [-Source] <IResource> -Check [<CommonParameters>]
```

## DESCRIPTION
Update InventorySources.
Multiple InventorySources in the Inventory may be udpated, when an Inventory is specified bye `-Inventory` parameter.

This command only sends a request to update InventorySources, not wait for the job is completed.
So, the returned job object will be non-completed status.
Use `Wait-AnsibleUnifiedJob` command to wait for the job to complete later.

Implementation of following API:  
- `/api/v2/inventory_sources/{id}/update/`  
- `/api/v2/inventries/{id}/update_inventory_sources/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Start-AnsibleInventoryUpdate -Source InventorySource:11

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
Parameter Sets: Check
Aliases:

Required: True
Position: Named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### -Source
Resource object to be updated or checked.

The resource is accepted following types:  
- `Inventory`  
- `InventorySource`

If the value is `Inventory`, all of InventorySources in the Inventory will be updated or checked.

> [!TIP]  
> Can specify the resource as string like `Inventory:1` (Format: `{Type}:{Id}`).
> And also accept objects have `type` and `id` properties.  
>
> For example:  
>  - `-Source (Get-AnsibleInventory -Id 1)`  
>  - `-Source @{ type = "inventory"; id = 1 }`  
>  - `-Source inventory:1`

```yaml
Type: IResource
Parameter Sets: (All)
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

### Jagabata.Resources.IResource
Resource object to be updated or checked.
See: `-Source` parameter.

## OUTPUTS

### Jagabata.Resources.InventoryUpdateJob+Detail
InventoryUpdate job object (non-completed status)

### System.Management.Automation.PSObject
Results of checked wheter the InventorySources can be updated.

## NOTES

## RELATED LINKS

[Invoke-AnsibleInventoryUpdate](Invoke-AnsibleInventoryUpdate)

[Get-AnsibleInventoryUpdateJob](Get-AnsibleInventoryUpdateJob.md)

[Find-AnsibleInventoryUpdateJob](Find-AnsibleInventoryUpdateJob.md)

[Wait-AnsibleUnifiedJob](Wait-AnsibleUnifiedJob.md)
