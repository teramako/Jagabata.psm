---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-AnsibleToken

## SYNOPSIS
Retrieve (OAuth2) AccessTokens by the ID(s).

## SYNTAX

```
Get-AnsibleToken [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve OAuth2 Access Tokens by the specified ID(s).

Implements following Rest API:  
- `/api/v2/tokens/{id}/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-AnsibleToken -Id 1

Id              Type Description               Created             Modified            Expires             User Username Application ApplicationName Scope
--              ---- -----------               -------             --------            -------             ---- -------- ----------- --------------- -----
 1 OAuth2AccessToken Admin PersonalAccessToken 2024/05/18 15:29:33 2024/05/18 15:29:33 3023/09/19 15:29:33    1 admin                                write
```

Retrieve an AccessToken for Database ID 1.

## PARAMETERS

### -Id
List of database IDs for one or more Tokens.

```yaml
Type: UInt64[]
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.UInt64[]
List of database IDs for one or more Tokens.

## OUTPUTS

### Jagabata.Resources.OAuth2AccessToken
## NOTES

## RELATED LINKS

[Find-AnsibleToken](Find-AnsibleToken.md)

[New-AnsibleToken](New-AnsibleToken.md)

[Update-AnsibleToken](Update-AnsibleToken.md)

[Remove-AnsibleToken](Remove-AnsibleToken.md)
