---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-AnsibleInventoryScript

## SYNOPSIS
Retrieve inventory scripts for the inventory.

## SYNTAX

### InventoryScript (Default)
```
Get-AnsibleInventoryScript [-Id] <UInt64[]> [-IncludeHostVars] [-IncludeDisabled] [-IncludeTowerVars]
 [<CommonParameters>]
```

### HostVariables
```
Get-AnsibleInventoryScript [-Id] <UInt64[]> [-Hostname] <String> [<CommonParameters>]
```

## DESCRIPTION
Retrieve inventory script  associated with the Inventory.

Implements following Rest API:  
- `/api/v2/inventories/{id}/script/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-AnsibleInventoryScript -Id 1 -IncludeHostVars | ConvertTo-Json -Depth 5
{
  "all": {
    "hosts": [
      "localhost"
    ],
    "children": [
      "DemoGroup"
    ]
  },
  "_meta": {
    "hostvars": {
      "localhost": {
        "ansible_connection": "local",
        "ansible_python_interpreter": "{{ ansible_playbook_python }}"
      }
    }
  }
}
```

Output as JSON format.

## PARAMETERS

### -Hostname
Get host variables for the specified hostname from the inventory script.

```yaml
Type: String
Parameter Sets: HostVariables
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
List of database IDs for one or more Inventories.

```yaml
Type: UInt64[]
Parameter Sets: (All)
Aliases: inventory

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -IncludeDisabled
By default, the inventory script will only return hosts that are enabled in the inventory.
This feature returns all hosts (including disabled ones).

```yaml
Type: SwitchParameter
Parameter Sets: InventoryScript
Aliases: all

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -IncludeHostVars
Include all host variables.
The `['_meta']['hostvars']` object in the response contains an entry for each host with its variables.

```yaml
Type: SwitchParameter
Parameter Sets: InventoryScript
Aliases: hostvars

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -IncludeTowerVars
Add variables to the hostvars of each host that specifies its enabled state and database ID.

`-IncludeHostVars` option will be enabled automatically when this option is enabled.

```yaml
Type: SwitchParameter
Parameter Sets: InventoryScript
Aliases: towervars

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.UInt64[]
List of database IDs for one or more Inventories.

## OUTPUTS

### System.Collections.Generic.Dictionary`2
The result of deserialized to `Dictionary<string, object?>`

## NOTES

## RELATED LINKS

[Get-AnsibleInventory](Get-AnsibleInventory.md)

[Find-AnsibleInventory](Find-AnsibleInventory.md)
