---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-Config

## SYNOPSIS
Retrieve various sitewide configuration settings.

## SYNTAX

```
Get-Config [<CommonParameters>]
```

## DESCRIPTION
Retrieve site configuration settings and general information.

Implements following Rest API:  
- `/api/v2/config/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-Config

TimeZone              : UTC
LicenseInfo           : { LicenseType = open, ValidKey = True, SubscriptionName = OPEN, ProductName = AWX }
Version               : 23.3.1
Eula                  :
ConfigAnalyticsStatus :
AnalyticsCollectors   : {[config, { ... }], [counts, { ... }], [cred_type_counts, { ... }], [events_table, { ... }]…}
BecomeMethods         : {sudo Sudo, su Su, pbrun Pbrun, pfexec Pfexec…}
UiNext                : True
ProjectBaseDir        : /var/lib/awx/projects/
ProjectLocalPaths     : {}
CustomVirtualenvs     : {}
```

## PARAMETERS

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None
## OUTPUTS

### Jagabata.Resources.Config
## NOTES

## RELATED LINKS
