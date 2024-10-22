---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-Application

## SYNOPSIS
Retrieve Applications by the ID(s).

## SYNTAX

```
Get-Application [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Implementation of following API:

* \`/api/v2/applications/{id}/\`

## EXAMPLES

### Example 1
```
PS C:\> Get-Application 1
```

Retrieve an Application of ID \`1\`.

## PARAMETERS

### -Id
List of database IDs for one or more Applications.

```yaml
Type: UInt64[]
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.UInt64[]
## OUTPUTS

### Jagabata.Resources.Application
## NOTES

## RELATED LINKS

[Get-Application](Get-Application.md)

[Find-Application](Find-Application.md)

[New-Application](New-Application.md)

[Update-Application](Update-Application.md)

[Remove-Application](Remove-Application.md)
