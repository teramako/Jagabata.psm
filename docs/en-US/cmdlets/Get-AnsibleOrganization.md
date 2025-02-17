---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-AnsibleOrganization

## SYNOPSIS
Retrieve Organizations by the ID(s).

## SYNTAX

```
Get-AnsibleOrganization [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve Organizations by the specified ID(s).

Implements following Rest API:  
- `/api/v2/organizations/{id}/`  

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-AnsibleOrganization -Id 1 | Format-List

Id                 : 1
Type               : Organization
Created            : 2023/11/04 16:20:27
Modified           : 2023/11/04 16:20:27
Name               : Default
Description        :
MaxHosts           : 0
CustomVirtualenv   :
InventoryCount     : 1
TeamCount          : 0
UserCount          : 1
JobTemplateCount   : 2
AdminCount         : 0
ProjectCount       : 1
DefaultEnvironment :
```

Retrieve an Organization for Database ID 1 and display as List format.

## PARAMETERS

### -Id
List of database IDs for one or more Organizations.

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
List of database IDs for one or more Organizations.

## OUTPUTS

### Jagabata.Resources.Organization
## NOTES

## RELATED LINKS

[Find-AnsibleOrganization](Find-AnsibleOrganization.md)

[New-AnsibleOrganization](New-AnsibleOrganization.md)

[Update-AnsibleOrganization](Update-AnsibleOrganization.md)

[Remove-AnsibleOrganization](Remove-AnsibleOrganization.md)
