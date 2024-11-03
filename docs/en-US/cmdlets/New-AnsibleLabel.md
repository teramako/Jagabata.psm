---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# New-AnsibleLabel

## SYNOPSIS
Create a Label.

## SYNTAX

```
New-AnsibleLabel -Name <String> -Organization <UInt64> [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Create a Label.

Implements following Rest API:  
- `/api/v2/labels/` (POST)

## EXAMPLES

### Example 1
```powershell
PS C:\> New-AnsibleLabel -Name TestLabel -Organization 1
```

Create a Label named "TestLabel" in Organization ID 1.

## PARAMETERS

### -Name
Label name

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Organization
Organization ID.

```yaml
Type: UInt64
Parameter Sets: (All)
Aliases:

Required: True
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

### System.String
Label name.

## OUTPUTS

### Jagabata.Resources.Label
New created Label object.

## NOTES

## RELATED LINKS

[Get-AnsibleLabel](Get-AnsibleLabel.md)

[Find-AnsibleLabel](Find-AnsibleLabel.md)

[Register-AnsibleLabel](Register-AnsibleLabel.md)

[Unregister-AnsibleLabel](Unregister-AnsibleLabel.md)

[Update-AnsibleLabel](Update-AnsibleLabel.md)
