---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-Credential

## SYNOPSIS
Retrieve Credentials by the ID(s).

## SYNTAX

```
Get-Credential [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve Credentials by the specified ID(s).

Implements following Rest API:  
- `/api/v2/credentials/{id}/`  

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-Credential -Id 1
```

Retrieve a Credential for Database ID 1.

## PARAMETERS

### -Id
List of database IDs for one or more Credentials.

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

### Jagabata.Resources.Credential
## NOTES

## RELATED LINKS

[Find-Credential](Find-Credential.md)

[New-Credential](New-Credential.md)

[Update-Credential](Update-Credential.md)

[Register-Credential](Register-Credential.md)

[Unregister-Credential](Unregister-Credential.md)

[Remove-Credential](Remove-Credential.md)
