---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-Me

## SYNOPSIS
Retrieve the current user.

## SYNTAX

```
Get-Me [<CommonParameters>]
```

## DESCRIPTION
Retrieve the current user.

Implements following Rest API:  
- `/api/v2/me/`  

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-Me

Id Type Username Email           FirstName LastName IsSuperuser IsSystemAuditor Created             Modified            LastLogin           LdapDn ExternalAccount
-- ---- -------- -----           --------- -------- ----------- --------------- -------             --------            ---------           ------ ---------------
 1 User admin    admin@localhost                           True           False 2023/11/04 16:20:25 2024/08/02 16:26:10 2024/08/02 16:26:10
```

## PARAMETERS

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None
## OUTPUTS

### Jagabata.Resources.User
## NOTES

## RELATED LINKS

[Get-User](Get-User.md)

[Find-User](Find-User.md)
