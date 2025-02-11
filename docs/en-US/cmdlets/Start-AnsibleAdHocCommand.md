---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Start-AnsibleAdHocCommand

## SYNOPSIS
Invoke (launch) an AdHocCommand.

## SYNTAX

```
Start-AnsibleAdHocCommand [-Target] <IResource> [-ModuleName] <String> [[-ModuleArgs] <String>]
 [-Credential] <UInt64> [-Limit <String>] [-Check] [<CommonParameters>]
```

## DESCRIPTION
Launch an AdHocCommand.
This command only sends a request to launch an AdHocCommand, not wait for the job is completed.
So, the returned job object will be non-completed status.
Use `Wait-AnsibleUnifiedJob` command to wait for the job to complete later.

Implementation of following API:  
- `/api/v2/inventories/{id}/ad_hoc_commands/`  
- `/api/v2/groups/{id}/ad_hoc_commands/`  
- `/api/v2/hosts/{id}/ad_hoc_commands/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-AnsibleGroup -Id 1 | Start-AnsibleAdHocCommand -ModuleName ping -Credential 1

 Id         Type Name JobType LaunchType Status Finished Elapsed LaunchedBy     Template Note
 --         ---- ---- ------- ---------- ------ -------- ------- ----------     -------- ----
120 AdHocCommand ping     run     Manual    New                0 [user][1]admin ping     {[ModuleArgs, ], [Limit, TestGroup], [Inventory, [2]TestInventory]}
```

Launch `ping` ansible module to the Group ID 1.

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

### -Limit
Further limit selected hosts to an additional pattern.

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

### -Target
Remote target resource to be executed.

The resource is accepted following types:  
- `Inventory`  
- `Group`  
- `Host`

> [!TIP]  
> Can specify the resource as string like `Group:1` (Format: `{Type}:{Id}`).
> And also accept objects have `type` and `id` properties.  
>
> For example:  
>  - `-Target (Get-AnsibleGroup -Id 1)`  
>  - `-Target @{ type = "group"; id = 1 }`  
>  - `-Target group:1`

```yaml
Type: IResource
Parameter Sets: (All)
Aliases: remote, r

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### Jagabata.IResource
Remote target resource to be executed.
See: `-Target` parameter.

## OUTPUTS

### Jagabata.Resources.AdHocCommand
AdHocCommand job object (non-completed status)

## NOTES

## RELATED LINKS

[Invoke-AnsibleAdHocCommand](Start-AnsibleAdHocCommand.md)

[Get-AnsibleAdHocCommand](Get-AnsibleAdHocCommand.md)

[Find-AnsibleAdHocCommand](Find-AnsibleAdHocCommand.md)

[Wait-AnsibleUnifiedJob](Wait-AnsibleUnifiedJob.md)
