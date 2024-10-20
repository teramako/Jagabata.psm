---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Invoke-InventoryUpdate

## SYNOPSIS
Invoke (update) an InventorySource and wait until the job is finished.

## SYNTAX

### Id
```
Invoke-InventoryUpdate [-IntervalSeconds <Int32>] [-SuppressJobLog] [-Id] <UInt64> [<CommonParameters>]
```

### Resource
```
Invoke-InventoryUpdate [-IntervalSeconds <Int32>] [-SuppressJobLog] [-Source] <IResource> [<CommonParameters>]
```

### CheckId
```
Invoke-InventoryUpdate [-Id] <UInt64> [-Check] [<CommonParameters>]
```

### CheckResource
```
Invoke-InventoryUpdate [-Source] <IResource> [-Check] [<CommonParameters>]
```

## DESCRIPTION
Update InventorySources and wait until the job is finished.
Multiple InventorySources in the Inventory may be udpated, when an Inventory is specified bye `-Inventory` parameter.

Implementation of following API:  
- `/api/v2/inventory_sources/{id}/update/`  
- `/api/v2/inventries/{id}/update_inventory_sources/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Invoke-InventoryUpdate -Id 11
====== [100] TestInventory - test_source ======
ansible-inventory [core 2.15.12]
  config file = /runner/project/ansible.cfg

(snip)

Parsed /runner/project/inventory/hosts.ini inventory source with ini plugin
    2.718 INFO     Processing JSON output...
    2.718 INFO     Loaded 1 groups, 2 hosts
    2.719 WARNING  loading into database...
    2.727 WARNING  group updates took 2 queries for 1 groups
    2.731 WARNING  host updates took 2 queries for 2 hosts
    2.731 WARNING  Group-group updates took 0 queries for 0 group-group relationships
    2.737 WARNING  Group-host updates took 3 queries for 1 group-host relationships
    2.745 WARNING  update computed fields took 8 queries
    2.746 WARNING  Inventory import completed for test_source in 0.0s
    2.746 WARNING  Inventory import required 26 queries taking 0.005s

 Id            Type Name                        JobType LaunchType     Status Finished            Elapsed LaunchedBy     Template        Note
 --            ---- ----                        ------- ----------     ------ --------            ------- ----------     --------        ----
100 InventoryUpdate TestInventory - test_source             Manual Successful 2024/08/06 14:51:07   2.751 [user][1]admin [11]test_source {[Inventory, [2]TestInventory], [Source, Scm], [SourcePath, inventory/hosts.ini]}
```

Update an InventorySource ID 11, and wait until for the job is finished.

### Example 2
```powershell
PS C:\> Get-Inventory -Id 2 | Invoke-InventoryUpdate -Check

Id            Type CanUpdate
--            ---- ---------
11 InventorySource      True
17 InventorySource      True
```

Check wheter InventorySources in the Inventory ID 2 can be updated.

## PARAMETERS

### -Check
Check wheter InventorySource(s) can be updated.

```yaml
Type: SwitchParameter
Parameter Sets: CheckId, CheckResource
Aliases:

Required: True
Position: Named
Default value: None
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

### -IntervalSeconds
Interval to confirm job completion (seconds).
Default is 5 seconds.

```yaml
Type: Int32
Parameter Sets: Id, Resource
Aliases:

Required: False
Position: Named
Default value: 5
Accept pipeline input: False
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

### -SuppressJobLog
Suppress display job log.

> [!TIP]  
> If you need the job log, use `-InformationVariable` parameter likes following:  
>  
>     PS C:\> Invoke-InventoryUpdate ... -SuppressJobLog -InformationVariable joblog  
>     (snip)  
>     PS C:\> $joblog  
>     ====== [100] TestInventory - test_source ======  
>     
>     (snip)

```yaml
Type: SwitchParameter
Parameter Sets: Id, Resource
Aliases:

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
InventorySource ID

### Jagabata.Resources.InventorySource
InventorySource object

### Jagabata.Resources.Inventory
Inventory object containing the InventorySources to be updated.

## OUTPUTS

### Jagabata.Resources.InventoryUpdateJob
The result job object of updated the InventorySource.

### System.Management.Automation.PSObject
Results of checked wheter the InventorySources can be updated.

## NOTES

## RELATED LINKS

[Start-InventoryUpdate](Start-InventoryUpdate.md)

[Get-InventoryUpdateJob](Get-InventoryUpdateJob.md)

[Find-InventoryUpdateJob](Find-InventoryUpdateJob.md)
