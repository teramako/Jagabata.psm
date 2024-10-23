---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Remove-AnsibleApplication

## SYNOPSIS
Remove an Application.

## SYNTAX

```
Remove-AnsibleApplication [-Id] <UInt64> [-Force] [-WhatIf] [-Confirm]
 [<CommonParameters>]
```

## DESCRIPTION
Remove an Application.

Implements following Rest API:  
- `/api/v2/applications/{id}/` (DELETE)  

## EXAMPLES

### Example 1
```powershell
PS C:\> Remove-AnsibleApplication -Id 3
```

## PARAMETERS

### -Force
Don't confirm. (Ignore `-Confirm` and `-WhatIf`)

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
Application ID to be removed.

```yaml
Type: UInt64
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
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

### System.UInt64
Application ID.

## OUTPUTS

### None
## NOTES

## RELATED LINKS

[Get-AnsibleApplication](Get-AnsibleApplication.md)

[Find-AnsibleApplication](Find-AnsibleApplication.md)

[New-AnsibleApplication](New-AnsibleApplication.md)

[Update-AnsibleApplication](Update-AnsibleApplication.md)

[Remove-AnsibleApplication](Remove-AnsibleApplication.md)
