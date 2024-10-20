---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-VariableData

## SYNOPSIS
Retrieve Variable Data

## SYNTAX

```
Get-VariableData [-Type] <ResourceType> [-Id] <UInt64> [<CommonParameters>]
```

## DESCRIPTION
Retrieve Variable Data for Inventory, Group or Host.

Implements following Rest API:  
- `/api/v2/inventories/{id}/variable_data/`  
- `/api/v2/groups/{id}/variable_data/`  
- `/api/v2/hosts/{id}/variable_data/` 

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-VariableData -Type Group -Id 1

Key                          Value
---                          -----
ansible_connection           ssh
ansible_ssh_private_key_file id_ed25519_ansible
ansible_user                 ansible
```

## PARAMETERS

### -Id
Datebase ID of the target resource.
Use in conjection with the `-Type` parameter.

```yaml
Type: UInt64
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Type
Resource type name of the target.
Use in conjection with the `-Id` parameter.

```yaml
Type: ResourceType
Parameter Sets: (All)
Aliases:
Accepted values: Inventory, Group, Host

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### Jagabata.Resources.ResourceType
Input by `Type` property in the pipeline object.

Acceptable values:  
- `Inventory`  
- `Group`  
- `Host`

### System.UInt64
Input by `Id` property in the pipeline object.

Database ID for the ResourceType

## OUTPUTS

### System.Collections.Generic.Dictionary`2[[System.String],[System.Object]]
All variables for the resource.

## NOTES

## RELATED LINKS

[Find-Inventory](Find-Inventory.md)

[Get-Inventory](Get-Inventory.md)

[Find-Group](Find-Group.md)

[Get-Group](Get-Group.md)

[Find-Host](Find-Host.md)

[Get-Host](Get-Host.md)
