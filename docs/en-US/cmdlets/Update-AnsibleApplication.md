---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Update-AnsibleApplication

## SYNOPSIS
Update an Application.

## SYNTAX

```
Update-AnsibleApplication [-Id] <UInt64> [-Name <String>] [-Description <String>] [-Organization <UInt64>]
 [-RedirectUris <String>] [-ClientType <ApplicationClientType>] [-SkipAuthorization] [-WhatIf] [-Confirm]
 [<CommonParameters>]
```

## DESCRIPTION
Update an Application. 

Implements following Rest API:  
- `/api/v2/applications/{id}/` (PATCH)

## EXAMPLES

### Example 1
```powershell
PS C:\> Update-AnsibleApplication -Id 3 -Name "ChangedName"
```

Change the name for the Application of Id 3.

## PARAMETERS

### -ClientType
Set to `Public` or `Confidential` depending on how secure the client device is.

```yaml
Type: ApplicationClientType
Parameter Sets: (All)
Aliases:
Accepted values: Confidential, Public

Required: False
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

### -Id
Application ID or it's resource to be update.

```yaml
Type: UInt64
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Name
Name of the Application.

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

### -Organization
Organization ID.

```yaml
Type: UInt64
Parameter Sets: (All)
Aliases:

Required: False
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

### System.UInt64
Application ID or it's resource to be update.
See: `-Id` parameter.

## OUTPUTS

### Jagabata.Resources.Application
Updated Application object.

## NOTES

## RELATED LINKS

[Get-AnsibleApplication](Get-AnsibleApplication.md)

[Find-AnsibleApplication](Find-AnsibleApplication.md)

[New-AnsibleApplication](New-AnsibleApplication.md)

[Remove-AnsibleApplication](Remove-AnsibleApplication.md)
