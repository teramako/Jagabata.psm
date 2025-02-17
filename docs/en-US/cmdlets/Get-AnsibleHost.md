---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-AnsibleHost

## SYNOPSIS
Retrieve Hosts by the ID(s).

## SYNTAX

```
Get-AnsibleHost [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve Hosts by the specified ID(s).

Implements following Rest API:  
- `/api/v2/hosts/{id}/`  

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-AnsibleHost -Id 1
```

Retrieve a Host for Database ID 1.

## PARAMETERS

### -Id
List of database IDs for one or more Hosts.

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
List of database IDs for one or more Hosts.

## OUTPUTS

### Jagabata.Resources.Host
## NOTES

## RELATED LINKS

[Find-AnsibleHost](Find-AnsibleHost.md)

[New-AnsibleHost](New-AnsibleHost.md)

[Update-AnsibleHost](Update-AnsibleHost.md)

[Register-AnsibleHost](Register-AnsibleHost.md)

[Unregister-AnsibleHost](Unregister-AnsibleHost.md)

[Remove-AnsibleHost](Remove-AnsibleHost.md)
