---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-AnsibleCredentialType

## SYNOPSIS
Retrieve CredentialTypes by the ID(s).

## SYNTAX

```
Get-AnsibleCredentialType [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve CredentialTypes by the specified ID(s).

Implements following Rest API:  
- `/api/v2/credential_types/{id}/`  

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-AnsibleCredentialType -Id 1
```

Retrieve a CredentialType for Database ID 1.

## PARAMETERS

### -Id
List of database IDs for one or more CredentialType.

```yaml
Type: UInt64[]
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.UInt64[]
One or more database IDs.

## OUTPUTS

### Jagabata.Resources.CredentialType
## NOTES

## RELATED LINKS

[Find-AnsibleCredentialType](Find-AnsibleCredentialType.md)

[New-AnsibleCredentialType](New-AnsibleCredentialType.md)

[Update-AnsibleCredentialType](Update-AnsibleCredentialType.md)

[Remove-AnsibleCredentialType](Remove-AnsibleCredentialType.md)