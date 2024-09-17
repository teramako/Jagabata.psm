---
external help file: AWX.psm.dll-Help.xml
Module Name: AWX.psm
online version:
schema: 2.0.0
---

# Update-Organization

## SYNOPSIS
Update an Organization.

## SYNTAX

```
Update-Organization [-Id] <UInt64> [-Name <String>] [-Description <String>] [-MaxHosts <UInt32>]
 [-DefaultEnvironment <UInt64>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Update an Organization. 

Implements following Rest API:  
- `/api/v2/organizations/{id}/` (PATCH)

## EXAMPLES

### Example 1
```powershell
PS C:\> Update-Organization -Id 2 -Name "ChangedName"
```

Change the name for the Organization of Id 2.

## PARAMETERS

### -DefaultEnvironment
ExecutionEnvironment ID.

Specify `0` or `$null` if want to set empty.

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

### -Id
Organization ID to be updated.

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

### AWX.Resources.Organization
New created Organization object.

## NOTES

## RELATED LINKS

[Get-Organization](Get-Organization.md)

[Find-Organization](Find-Organization.md)

[New-Organization](New-Organization.md)

[Remove-Organization](Remove-Organization.md)