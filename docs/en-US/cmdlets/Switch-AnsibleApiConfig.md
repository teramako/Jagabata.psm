---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Switch-AnsibleApiConfig

## SYNOPSIS
Switch to anothor config.

## SYNTAX

```
Switch-AnsibleApiConfig [[-Path] <String>] [<CommonParameters>]
```

## DESCRIPTION
Load another configuration file to switching AWX/AnsibleTower URL or AccessToken.
Default path is used when `-Path` parameter is ommitted or the value is empty.

## EXAMPLES

### Example 1
```powershell
PS C:\> Switch-AnsibleApiConfig ~\.another_config.json

Origin                 LastSaved          File
------                 ---------          ----
http://localhost:8013/ 2024/06/15 9:49:08 C:\Users\***\.another_config.json
```

## PARAMETERS

### -Path
Configuration file path.
Default configuration path is used if the path is empty.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 0
Default value: ""
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None
## OUTPUTS

### Jagabata.ApiConfig
Loaded configuration object.

## NOTES

## RELATED LINKS

[New-AnsibleApiConfig](New-AnsibleApiConfig.md)

[Get-AnsibleApiConfig](Get-AnsibleApiConfig.md)
