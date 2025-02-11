---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-AnsibleVariableData

## SYNOPSIS
Retrieve Variable Data

## SYNTAX

```
Get-AnsibleVariableData [-Resource] <IResource> [<CommonParameters>]
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
PS C:\> Get-AnsibleVariableData -Type Group -Id 1

Key                          Value
---                          -----
ansible_connection           ssh
ansible_ssh_private_key_file id_ed25519_ansible
ansible_user                 ansible
```

## PARAMETERS

### -Resource
Resource object from which has variables data.

The resource is an object with `Id` and `Type` properties.
And `Type` should be following value:  
- `Inventory`  
- `Group`  
- `Host`

> [!TIP]  
> Can specify the resource as string like `Host:1` (Format: `{Type}:{Id}`).
> And also accept objects have `type` and `id` properties.  
>
> For example:  
>  - `-Resource (Get-AnsibleHost -Id 1)`  
>  - `-Resource @{ type = "host"; id = 1 }`  
>  - `-Resource host:1`

```yaml
Type: IResource
Parameter Sets: (All)
Aliases: associatedWith, r

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
Resource object from which has variables data.
See `-Resource` parameter.

## OUTPUTS

### System.Collections.Generic.Dictionary`2[[System.String],[System.Object]]
All variables for the resource.

## NOTES

## RELATED LINKS

[Find-AnsibleInventory](Find-AnsibleInventory.md)

[Get-AnsibleInventory](Get-AnsibleInventory.md)

[Find-AnsibleGroup](Find-AnsibleGroup.md)

[Get-AnsibleGroup](Get-AnsibleGroup.md)

[Find-AnsibleHost](Find-AnsibleHost.md)

[Get-AnsibleHost](Get-AnsibleHost.md)
