---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# New-AnsibleOrganization

## SYNOPSIS
Create an Organization.

## SYNTAX

```
New-AnsibleOrganization [-Name] <String> [-Description <String>] [-MaxHosts <UInt32>]
 [-DefaultEnvironment <UInt64>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Create an Organization.

Implements following Rest API:  
- `/api/v2/organizations/` (POST)

## EXAMPLES

### Example 1
```powershell
PS C:\> New-AnsibleOrganization -Name Org2
```

## PARAMETERS

### -DefaultEnvironment
ExecutionEnvironment ID.

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
Optional description of the Organization.

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

### -MaxHosts
Maximum number of hosts allowed to be managed by the Organization.

```yaml
Type: UInt32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
Name of the Organization.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
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
Shows what would happen if the cmdlet runs. The cmdlet is not run.

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

### Jagabata.Resources.Organization
New created Organization object.

## NOTES

## RELATED LINKS

[Get-AnsibleOrganization](Get-AnsibleOrganization.md)

[Find-AnsibleOrganization](Find-AnsibleOrganization.md)

[Update-AnsibleOrganization](Update-AnsibleOrganization.md)

[Remove-AnsibleOrganization](Remove-AnsibleOrganization.md)
