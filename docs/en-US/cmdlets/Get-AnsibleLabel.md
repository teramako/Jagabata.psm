---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-AnsibleLabel

## SYNOPSIS
Retrieve Labels by the ID(s).

## SYNTAX

```
Get-AnsibleLabel [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve Labels by the specified ID(s).

Implements following Rest API:  
- `/api/v2/labels/{id}/`  

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-AnsibleLabel -Id 1
```

Retrieve a Label for Database ID 1.

## PARAMETERS

### -Id
List of database IDs for one or more Labels.

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
List of database IDs for one or more Labels.

## OUTPUTS

### Jagabata.Resources.Label
## NOTES

## RELATED LINKS

[Find-AnsibleLabel](Find-AnsibleLabel.md)

[New-AnsibleLabel](New-AnsibleLabel.md)

[Register-AnsibleLabel](Register-AnsibleLabel.md)

[Unregister-AnsibleLabel](Unregister-AnsibleLabel.md)

[Update-AnsibleLabel](Update-AnsibleLabel.md)
