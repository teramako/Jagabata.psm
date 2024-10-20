---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-Setting

## SYNOPSIS
Retrieve Settings.

## SYNTAX

```
Get-Setting [[-Name] <String>] [<CommonParameters>]
```

## DESCRIPTION
Retrieve Settings of the specified name or retreive a list of its names when ommited.

Implements following Rest API:  
- `/api/v2/settings/`  
- `/api/v2/settings/{category_slug}/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-Setting

Url                                      Slug                   Name
---                                      ----                   ----
/api/v2/settings/all/                    all                    All
/api/v2/settings/authentication/         authentication         Authentication
/api/v2/settings/azuread-oauth2/         azuread-oauth2         Azure AD OAuth2
(snip)
/api/v2/settings/system/                 system                 System
/api/v2/settings/tacacsplus/             tacacsplus             TACACS+
/api/v2/settings/ui/                     ui                     UI
```

Retreive list of Settings name.

### Example 2
```powershell
PS C:\> Get-Setting -name github

Key                                 Value
---                                 -----
SOCIAL_AUTH_GITHUB_CALLBACK_URL     http://localhost:8013/sso/complete/github/
SOCIAL_AUTH_GITHUB_KEY
SOCIAL_AUTH_GITHUB_SECRET
SOCIAL_AUTH_GITHUB_ORGANIZATION_MAP
SOCIAL_AUTH_GITHUB_TEAM_MAP
```

Retreive list of Settings name.

## PARAMETERS

### -Name
The setting name

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
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

### Jagabata.Resources.Setting
## NOTES

## RELATED LINKS
