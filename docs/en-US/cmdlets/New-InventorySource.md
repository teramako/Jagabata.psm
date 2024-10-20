---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# New-InventorySource

## SYNOPSIS
Create an InventorySource.

## SYNTAX

```
New-InventorySource -Name <String> [-Description <String>] -Inventory <UInt64> -Source <InventorySourceSource>
 [-SourceProject <UInt64>] [-SourcePath <String>] [-SourceVars <String>] [-ScmBranch <String>]
 [-Credential <UInt64>] [-EnabledVar <String>] [-EnabledValue <String>] [-HostFilter <String>] [-Overwrite]
 [-OverwriteVars] [-Timeout <Int32>] [-Verbosity <Int32>] [-Limit <String>] [-ExecutionEnvironment <UInt64>]
 [-UpdateOnLaunch] [-UpdateCacheTimeout <Int32>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Create an InventorySource.

Implements following Rest API:  
- `/api/v2/inventory_sources/` (POST)

## EXAMPLES

### Example 1
```powershell
PS C:\> New-InventorySource -Name DemoSource -Inventory 1 -Source Scm -SourceProject 10 -SourcePath inventory/hosts.ini
```

## PARAMETERS

### -Credential
Cloud credential ID to use for inventory updates.

```yaml
Type: UInt64
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Description
Optional description of the InventorySource.

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

### -EnabledValue
Only used when enabled_var is set.
Value when the host is considered enabled.
For example if `enabled_var="status.power_state" and enabled_value="powered_on"` with host variables:
`{ "status": { "power_state": "powered_on", "created": "2018-02-01T08:00:00.000000Z:00", "healthy": true }, "name": "foobar", "ip_address": "192.168.2.1"}`
The host would be marked enabled.
If `power_state` where any value other than `powered_on` then the host would be disabled when imported.
If the key is not found then the host will be enabled.

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

### -EnabledVar
Retrieve the enabled state from the given dict of host variables.
The enabled variable may be specified as "foo.bar", in which case the lookup will traverse into nested dicts,
equivalent to: `from_dict.get("foo", {}).get("bar", default)`

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

### -ExecutionEnvironment
ExecutionEnvironemnt ID.

```yaml
Type: UInt64
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -HostFilter
This field is deprecated and will be removed in a future release.
Regex where only matching hosts will be imported.

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

### -Inventory
Inventory ID.

```yaml
Type: UInt64
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Limit
Enter host, group or pattern match.

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

### -Name
Name of the InventorySource.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Overwrite
Overwrite local groups and hosts from remote InventorySource.

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

### -OverwriteVars
Overwrite local variables from remote InventorySource.

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

### -ScmBranch
InventorySource SCM branch.
Project default used if blank.
Only allowed if project `allow_override` field is set to true.

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

### -Source
Source Type:

- `File`: File, Directory or Script  
- `Constructed`: Template additional groups and hostvars at runtime  
- `Scm`: Sourced from a Project  
- `EC2`: Amazon EC2  
- `GCE`: Google Compute Engine  
- `AzureRM`: Microsoft Azure Resource Manager  
- `VMware`: VMware vCenter  
- `Satellite6`: Red Hat Satellite 6  
- `OpenStack`: OpenStack  
- `RHV`: Red Hat Virtualization  
- `Controller`: Red Hat Ansible Automation Platform  
- `Insights`: Red Hat Insights

```yaml
Type: InventorySourceSource
Parameter Sets: (All)
Aliases:
Accepted values: File, Constructed, Scm, EC2, GCE, AzureRM, VMware, Satellite6, OpenStack, RHV, Controller, Insights

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SourcePath
Inventory directory, file, or script to load.

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

### -SourceProject
Project containing inventory file used as source.

```yaml
Type: UInt64
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SourceVars
InventorySource variables in YAML or JSON format.

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

### -Timeout
The amount of time (in seconds) to run before the task is canceled.

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -UpdateCacheTimeout

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -UpdateOnLaunch

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

### -Verbosity

- `0`: Warning  
- `1`: Info (Default)  
- `2`: Debug

```yaml
Type: Int32
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

### Jagabata.Resources.InventorySource
New created InventorySource object.

## NOTES

## RELATED LINKS

[Get-InventorySource](Get-InventorySource.md)

[Find-InventorySource](Find-InventorySource.md)

[Update-InventorySource](Update-InventorySource.md)

[Remove-InventorySource](Remove-InventorySource.md)
