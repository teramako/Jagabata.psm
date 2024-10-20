---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-ApiConfig

## SYNOPSIS
Get loaded config data currently.

## SYNTAX

```
Get-ApiConfig [<CommonParameters>]
```

## DESCRIPTION
Get this module's config using currently.
The config has AWX/AnsibleTower URL, file path and token value.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-ApiConfig
```

## PARAMETERS

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None
## OUTPUTS

### Jagabata.ApiConfig
## NOTES

## RELATED LINKS

[New-ApiConfig](New-ApiConfig.md)

[Switch-ApiConfig](Switch-ApiConfig.md)
