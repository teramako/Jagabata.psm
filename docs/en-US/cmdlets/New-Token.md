---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# New-Token

## SYNOPSIS
Create an AccessToken.

## SYNTAX

### Application (Default)
```
New-Token [-Application] <UInt64> [-Scope <String>] [-Description <String>] [-WhatIf] [-Confirm]
 [<CommonParameters>]
```

### User
```
New-Token [-ForMe] [[-Application] <UInt64>] [-Scope <String>] [-Description <String>] [-WhatIf] [-Confirm]
 [<CommonParameters>]
```

## DESCRIPTION
Create an OAuth2 AccessToken.

> [!WARNING]  
> Be sure to store the result into a variable.  
> Otherwise, you will not be able to obtain the generated token value.

Implements following Rest API:  
- `/api/v2/applications/{id}/tokens/` (POST)  
- `/api/v2/users/0/tokens/` (POST)

## EXAMPLES

### Example 1
```powershell
PS C:\> $accessToken = New-Token -Application (Get-Application -Id 2) -Scope write
PS C:\> $accessToken.Token
```

Create an AccessToken for the Application of ID 2, and show the created token.

### Example 2
```powershell
PS C:\> $pat = New-Token -ForMe -Scope write -Description "Parsonal Access Token"
PS C:\> $pat.Token
```

Create a Parsonal Access Token.

## PARAMETERS

### -Application
Application ID.

```yaml
Type: UInt64
Parameter Sets: Application
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

```yaml
Type: UInt64
Parameter Sets: User
Aliases:

Required: False
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Description
Optional description of the AccessToken.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ForMe
Create an AccessToken for yourself.

```yaml
Type: SwitchParameter
Parameter Sets: User
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Scope
Allowed scopes, further restricts user's permission.

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: read, write

Required: False
Position: Named
Default value: write
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

### None
## OUTPUTS

### Jagabata.Resources.OAuth2AccessToken
New created AccessToken object.

## NOTES

## RELATED LINKS

[Get-Token](Get-Token.md)

[Find-Token](Find-Token.md)

[Update-Token](Update-Token.md)

[Remove-Token](Remove-Token.md)
