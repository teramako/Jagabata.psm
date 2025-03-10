---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# New-AnsibleCredential

## SYNOPSIS
Create a Credential.

## SYNTAX

```
New-AnsibleCredential -CredentialType <UInt64> -Name <String> [-Description <String>] [-Inputs <IDictionary>]
 [-Owner <IResource>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Create a Credential.

Implements following Rest API:  
- `/api/v2/credentials/` (POST)

## EXAMPLES

### Example 1
```powershell
PS C:\> $inputs = @{ username = "foo_user"; password = "P@ssw0rd" }
PS C:\> New-AnsibleCredential -CredentialType 1 -Name Foo-Cred -Input $inputs -Owner (Get-AnsibleOrganization -Id 2)
```

## PARAMETERS

### -CredentialType
CredentialType ID.

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

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Owner
Grant role of the specified resource as owner.
If ommited, used User ID of the current config.

```yaml
Type: IResource
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

### Jagabata.Resources.Credential
New created Credential object.

## NOTES

## RELATED LINKS

[Get-AnsibleCredential](Get-AnsibleCredential.md)

[Find-AnsibleCredential](Find-AnsibleCredential.md)

[Update-AnsibleCredential](Update-AnsibleCredential.md)

[Register-AnsibleCredential](Register-AnsibleCredential.md)

[Unregister-AnsibleCredential](Unregister-AnsibleCredential.md)

[Remove-AnsibleCredential](Remove-AnsibleCredential.md)
