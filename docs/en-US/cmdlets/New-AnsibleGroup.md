---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# New-AnsibleGroup

## SYNOPSIS
Create a Group.

## SYNTAX

```
New-AnsibleGroup [-Inventory] <UInt64> [-Name] <String> [-Description <String>] [-Variables <String>] [-WhatIf]
 [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Create a Group.

Implements following Rest API:  
- `/api/v2/groups/` (POST)

## EXAMPLES

### Example 1
```powershell
PS C:\> New-AnsibleGroup -Inventory 1 -Name group_name
```

Create a new Group into the Inventory of ID 1.

## PARAMETERS

### -Description
Optional description of the group.

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
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
Name of the group.

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

### -Variables
Specify in JSON or YAML format.
You can also specify an object of type `IDictionary` as a parameter value.

Example: `-Variables @{ ansible_connection = "ssh"; ansible_user = "ssh_user" }`

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

### Jagabata.Resources.Group
New created Group object.

## NOTES

## RELATED LINKS

[Get-AnsibleGroup](Get-AnsibleGroup.md)

[Find-AnsibleGroup](Find-AnsibleGroup.md)

[Add-Group](Add-Group.md)

[Update-AnsibleGroup](Update-AnsibleGroup.md)

[Remove-AnsibleGroup](Remove-AnsibleGroup.md)
