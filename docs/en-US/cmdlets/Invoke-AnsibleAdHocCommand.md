---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Invoke-AnsibleAdHocCommand

## SYNOPSIS
Invoke (launch) an AdHocCommand and wait until the job is finished.

## SYNTAX

### Host
```
Invoke-AnsibleAdHocCommand [-IntervalSeconds <Int32>] [-SuppressJobLog] [-Host] <Host> [-ModuleName] <String>
 [[-ModuleArgs] <String>] [-Credential] <UInt64> [-Check] [<CommonParameters>]
```

### Group
```
Invoke-AnsibleAdHocCommand [-IntervalSeconds <Int32>] [-SuppressJobLog] [-Group] <Group> [-ModuleName] <String>
 [[-ModuleArgs] <String>] [-Credential] <UInt64> [-Check] [<CommonParameters>]
```

### Inventory
```
Invoke-AnsibleAdHocCommand [-IntervalSeconds <Int32>] [-SuppressJobLog] [-Inventory] <Inventory>
 [-ModuleName] <String> [[-ModuleArgs] <String>] [-Credential] <UInt64> [-Limit <String>] [-Check]
 [<CommonParameters>]
```

### InventoryId
```
Invoke-AnsibleAdHocCommand [-IntervalSeconds <Int32>] [-SuppressJobLog] [-InventoryId] <UInt64>
 [-ModuleName] <String> [[-ModuleArgs] <String>] [-Credential] <UInt64> [-Limit <String>] [-Check]
 [<CommonParameters>]
```

## DESCRIPTION
Launch an AdHocCommand and wait until the job is finished.

Implementation of following API:  
- `/api/v2/inventories/{id}/ad_hoc_commands/`  
- `/api/v2/groups/{id}/ad_hoc_commands/`  
- `/api/v2/hosts/{id}/ad_hoc_commands/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-AnsibleGroup -Id 1 | Invoke-AnsibleAdHocCommand -ModuleName ping -Credential 1
====== [20] ping ======
localhost | SUCCESS => {
    "changed": false,
    "ping": "pong"
}
hostA | UNREACHABLE! => {
    "changed": false,
    "msg": "Failed to connect to the host via ssh: ssh: connect to host ***.***.***.*** port 22: Connection timed out",
    "unreachable": true
}

Id         Type Name JobType LaunchType Status Finished            Elapsed LaunchedBy     Template Note
--         ---- ---- ------- ---------- ------ --------            ------- ----------     -------- ----
20 AdHocCommand ping     run     Manual Failed 2024/08/06 12:14:35  11.651 [user][1]admin ping     {[ModuleArgs, ], [Limit, TestGroup], [Inventory, [2]TestInventory]}
```

Launch `ping` ansible module to the Group ID 1, and wait until for the job is finished.

## PARAMETERS

### -Check
Luanch as `Check` mode.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### -Credential
Credential ID of Machine type for executing command to the remote hosts.

```yaml
Type: UInt64
Parameter Sets: (All)
Aliases:

Required: True
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Group
Group to be executed.

```yaml
Type: Group
Parameter Sets: Group
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Host
Host to be executed.

```yaml
Type: Host
Parameter Sets: Host
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
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: 5
Accept pipeline input: False
Accept wildcard characters: False
```

### -Inventory
Inventory to be executed.

```yaml
Type: Inventory
Parameter Sets: Inventory
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -InventoryId
Inventory ID to be executed.

```yaml
Type: UInt64
Parameter Sets: InventoryId
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Limit
Further limit selected hosts to an additional pattern.

```yaml
Type: String
Parameter Sets: Inventory, InventoryId
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ModuleArgs
The action's (`-ModuleName`) options in space separated `k=v` format or JSON format.

e.g.)  
- `opt1=val1 opt=2=val2`  
- `{"opt1": "val1", "opt2": "val2"}`

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ModuleName
Name of the action to execute.

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

### -SuppressJobLog
Suppress display job log.

> [!TIP]  
> If you need the job log, use `-InformationVariable` parameter likes following:  
>  
>     PS C:\> Invoke-AnsibleAdHocCommand ... -InformationVariable joblog  
>     (snip)  
>     PS C:\> $joblog  
>     ====== [30] ping ======  
>  
>     localhost | SUCCESS => {  
>         "changed": false,  
>         "ping": "pong"  
>     }

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### Jagabata.Resources.Host
Host object to be executed.

### Jagabata.Resources.Group
Group object to be executed.

### Jagabata.Resources.Inventory
Inventory object to be executed.

### System.UInt64
Inventory ID to be executed.

## OUTPUTS

### Jagabata.Resources.AdHocCommand
AdHocCommand job object (non-completed status)

## NOTES

## RELATED LINKS

[Start-AnsibleAdHocCommand](Start-AnsibleAdHocCommand.md)

[Get-AnsibleAdHocCommand](Get-AnsibleAdHocCommand.md)

[Find-AnsibleAdHocCommand](Find-AnsibleAdHocCommand.md)
