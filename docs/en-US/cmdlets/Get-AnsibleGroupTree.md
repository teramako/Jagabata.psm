---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-AnsibleGroupTree

## SYNOPSIS
Retrieve group hierarchies for an inventory.

## SYNTAX

```
Get-AnsibleGroupTree [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve group hierarchies associated with the Inventory.

Implements following Rest API:  
- `/api/v2/inventories/{id}/tree/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-AnsibleGroupTree -Id 1

# Inventory: [1] TestInventory

- id: 2
  name: Linux
  description: Linux group
  variables: |
    {"ansible_connection": "ssh", "ansible_ssh_private_key_file": "id_ed25519_ansible", "ansible_user": "ansible"}
  children:
    - id: 4
      name: Ubuntu
      description: Ubuntu Machines
    - id: 5
      name: RedHat
      description: Red Hat Linux Machines


- id: 3
  name: Windows
  variables: |
    {"ansible_connection": "psrp", "ansible_user": "ansible"}
```

By default, the group hierarchy is displayed in a format similar to YAML.

### Example 2
```powershell
PS C:\> Get-AnsibleGroupTree -Id 1 | Format-List

   Inventory: [1] TestInventory

Type        : Group
Id          : 2
Created     : 2024/06/07 21:50:51
Modified    : 2024/06/07 21:50:51
Name        : Linux
Description : Linux group
Variables   : {"ansible_connection": "ssh", "ansible_ssh_private_key_file": "id_ed25519_ansible", "ansible_user": "ansible"}
Children    : {Ubuntu, RedHat}

Type        : Group
Id          : 3
Created     : 2024/06/07 21:50:51
Modified    : 2024/06/07 21:50:51
Name        : Windows
Description :
Variables   : {"ansible_connection": "psrp", "ansible_user": "ansible"}
Children    : {ChildGroup}
```

Show as list-format.

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
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.UInt64[]
List of database IDs for one or more Inventories.

## OUTPUTS

### Jagabata.Resources.Group+Tree
Almost same as `Jagabata.Resources.Group` object, but with the `Children` property added.

## NOTES

## RELATED LINKS

[Get-AnsibleGroup](Get-AnsibleGroup.md)

[Find-AnsibleGroup](Find-AnsibleGroup.md)

[Get-AnsibleInventory](Get-AnsibleInventory.md)

[Find-AnsibleInventory](Find-AnsibleInventory.md)
