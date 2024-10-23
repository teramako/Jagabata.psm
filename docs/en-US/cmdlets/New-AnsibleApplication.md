---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# New-AnsibleApplication

## SYNOPSIS
Create an Application.

## SYNTAX

```
New-AnsibleApplication [-Name] <String> [-Description <String>] -Organization <UInt64>
 -AuthorizationGrantType <String> [-RedirectUris <String>] -ClientType <ApplicationClientType>
 [-SkipAuthorization] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Create an Application.

Implements following Rest API:  
- `/api/v2/applications/` (POST)

## EXAMPLES

### Example 1
```powershell
PS C:\> New-AnsibleApplication -Name AppName -Organization 1 -AuthorizationGrantType authorization-code -ClientType Confidential
```

## PARAMETERS

### -AuthorizationGrantType
The Grant type the user must use for acquire tokens for the Application.

- `authorization-code`: Authorization Code  
- `password`: Resource owner password-based

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: password, authorization-code

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ClientType
Set to `Public` or `Confidential` depending on how secure the client device is.

```yaml
Type: ApplicationClientType
Parameter Sets: (All)
Aliases:
Accepted values: Confidential, Public

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Description
Optional description of the Application.

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

### -Name
Name of the Application.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Organization
Organization ID.

```yaml
Type: UInt64
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -RedirectUris
Allowed URIs list, space separated.

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

### -SkipAuthorization
Set to skip authorization step for completely trusted application.

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

### Jagabata.Resources.Application
New created Application object.

## NOTES

## RELATED LINKS

[Get-AnsibleApplication](Get-AnsibleApplication.md)

[Find-AnsibleApplication](Find-AnsibleApplication.md)

[Update-AnsibleApplication](Update-AnsibleApplication.md)

[Remove-AnsibleApplication](Remove-AnsibleApplication.md)
