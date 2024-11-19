---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# New-AnsibleCredentialType

## SYNOPSIS
Create a CredentialType.

## SYNTAX

### InputsDict
```
New-AnsibleCredentialType [-Name] <String> [-Description <String>] [-Kind] <String> -Inputs <IDictionary>
 [-Injectors <IDictionary>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

### FieldList
```
New-AnsibleCredentialType [-Name] <String> [-Description <String>] [-Kind] <String> -FieldList <FieldList>
 [-Injectors <IDictionary>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Create a CredentialType.

Implements following Rest API:  
- `/api/v2/credential_types/` (POST)

## EXAMPLES

### Example 1
```powershell
PS C:\> $inputs = @{ fields = @( @{ id="pass"; label="Password"; secret = $true } ); required = @("pass") }
PS C:\> $injectors = @{ env = @{ FOO_PASSWORD = "{{pass}}" } }
PS C:\> New-AnsibleCredentialType -Name demoCredType -Kind net -Inputs $inputs -Injectors $injectors
```

## PARAMETERS

### -Description
Optional description of the CredentialType.

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

### -FieldList
Custom `inputs` of CredentialType which defined by `Jagabata.CredentialType.FieldList`

```yaml
Type: FieldList
Parameter Sets: FieldList
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Injectors

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

### -Inputs

```yaml
Type: IDictionary
Parameter Sets: InputsDict
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Kind

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: net, cloud

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
Name of the CredentialType.

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
Shows what would happen if the cmdlet runs. The cmdlet is not run.

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

### Jagabata.Resources.CredentialType
New created CredentialType object.

## NOTES

## RELATED LINKS

[Get-AnsibleCredentialType](Get-AnsibleCredentialType.md)

[Find-AnsibleCredentialType](Find-AnsibleCredentialType.md)

[Update-AnsibleCredentialType](Update-AnsibleCredentialType.md)

[Remove-AnsibleCredentialType](Remove-AnsibleCredentialType.md)
