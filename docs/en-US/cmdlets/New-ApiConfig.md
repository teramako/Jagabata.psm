---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# New-ApiConfig

## SYNOPSIS
Create config file that should be used by this module.

## SYNTAX

```
New-ApiConfig [-Uri] <Uri> [-SaveAs <FileInfo>] [<CommonParameters>]
```

## DESCRIPTION
Create a configuration file which is stored AWX/AnsibleTower URL, OAuth2 AccessToken and etc...
This will be the first thing you should do when using this module.

## EXAMPLES

### Example 1
```powershell
PS C:\> New-ApiConfig -Uri http://localhost:8013/
    _        _    ____
   / \      / \  |  _ \       _ __  ___ _ __ ___
  / _ \    / _ \ | |_) |     | '_ \/ __| '_ ` _ \
 / ___ \  / ___ \|  __/   _  | |_) \__ \ | | | | |
/_/   \_\/_/   \_\_|     (_) | .__/|___/_| |_| |_|
                             |_|

Please enter the your Personal Access Token(PAT)
Personal Token: ******************************
Try to retrieve the user information from: http://localhost:8013/
Success: ******(*****@*****.***)
Save config to: C:\Users\*****\.ansible_api_config.json

Sccess 🎉
Origin                 LastSaved          File
------                 ---------          ----
http://localhost:8013/ 2024/08/06 7:42:02 C:\Users\*****\.ansible_api_config.json
```

Create new configuration file.

## PARAMETERS

### -SaveAs
Configuration file path

```yaml
Type: FileInfo
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Uri
AWX/AnsibleTower URL

```yaml
Type: Uri
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None
## OUTPUTS

### Jagabata.ApiConfig
Newly created configuration object.

## NOTES

## RELATED LINKS

[Get-ApiConfig](Get-ApiConfig.md)

[Switch-ApiConfig](Switch-ApiConfig.md)
