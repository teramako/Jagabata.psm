---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-AnsibleGroup

## SYNOPSIS
Retrieve Groups by the ID(s).

## SYNTAX

```
Get-AnsibleGroup [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve Groups by the specified ID(s).

Implements following Rest API:  
- `/api/v2/groups/{id}/`  

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-AnsibleGroup -Id 1
```

Retrieve a Group for Database ID 1.

## PARAMETERS

### -Id
List of database IDs for one or more Groups.

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
List of database IDs for one or more Groups.

## OUTPUTS

### Jagabata.Resources.Group
## NOTES

## RELATED LINKS

[Find-AnsibleGroup](Find-AnsibleGroup.md)

[New-AnsibleGroup](New-AnsibleGroup.md)

[Update-AnsibleGroup](Update-AnsibleGroup.md)

[Register-AnsibleGroup](Register-AnsibleGroup.md)

[Unregister-AnsibleGroup](Unregister-AnsibleGroup.md)

[Remove-AnsibleGroup](Remove-AnsibleGroup.md)
