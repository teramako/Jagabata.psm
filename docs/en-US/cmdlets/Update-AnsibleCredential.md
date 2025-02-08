---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Update-AnsibleCredential

## SYNOPSIS
Update a Credential.

## SYNTAX

```
Update-AnsibleCredential [-Id] <UInt64> [-Name <String>] [-Description <String>] [-CredentialType <UInt64>]
 [-Inputs <IDictionary>] [-Organization <UInt64>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Update a Credential. 

Implements following Rest API:  
- `/api/v2/credentials/{id}/` (PATCH)

## EXAMPLES

### Example 1
```powershell
PS C:\> Update-AnsibleCredential -Id 2 -Description "Updated Description"
```

## PARAMETERS

### -CredentialType
CredentialType ID or its resource object.

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

### -Description
Optional description of the Credential.

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
Credential ID or its resource object to be updated.

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

### -Inputs

```yaml
Type: IDictionary
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
Name of the Credential.

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
Credential ID or its resource object to be updated.
See `-Id` parameter.

## OUTPUTS

### Jagabata.Resources.Credential
Updated Credential object.

## NOTES

## RELATED LINKS

[Get-AnsibleCredential](Get-AnsibleCredential.md)

[Find-AnsibleCredential](Find-AnsibleCredential.md)

[New-AnsibleCredential](New-AnsibleCredential.md)

[Register-AnsibleCredential](Register-AnsibleCredential.md)

[Unregister-AnsibleCredential](Unregister-AnsibleCredential.md)

[Remove-AnsibleCredential](Remove-AnsibleCredential.md)
