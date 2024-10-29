---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Remove-AnsibleGroup

## SYNOPSIS
Remove a Group.

## SYNTAX

```
Remove-AnsibleGroup [-Id] <UInt64> [-Force] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Remove the Group.

Implements following Rest API:  
- `/api/v2/groups/{id}/` (DELETE)  

## EXAMPLES

### Example 1
```powershell
PS C:\> Remove-AnsibleGroup -Id 3
```

Delete completly the Group of ID 3.

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
Group Id.

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
Group Id.

## OUTPUTS

### None
## NOTES

## RELATED LINKS

[Get-AnsibleGroup](Get-AnsibleGroup.md)

[Find-AnsibleGroup](Find-AnsibleGroup.md)

[New-AnsibleGroup](New-AnsibleGroup.md)

[Update-AnsibleGroup](Update-AnsibleGroup.md)

[Register-AnsibleGroup](Register-AnsibleGroup.md)

[Unregister-AnsibleGroup](Unregister-AnsibleGroup.md)
