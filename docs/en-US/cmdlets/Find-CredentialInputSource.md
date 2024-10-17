---
external help file: AWX.psm.dll-Help.xml
Module Name: AWX.psm
online version:
schema: 2.0.0
---

# Find-CredentialInputSource

## SYNOPSIS
Retrieve CredentialsInputSources.

## SYNTAX

```
Find-CredentialInputSource [[-Credential] <UInt64>] [-OrderBy <String[]>] [-Search <String[]>]
 [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All] [<CommonParameters>]
```

## DESCRIPTION
Retrieve the list of CrdentialInputSources.

Implementation of following API:  
- `/api/v2/credential_input_sources/`  
- `/api/v2/credentials/{id}/input_sources/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Find-CredentialInputSource
```

### Example 2
```powershell
PS C:\> Find-CredentialInputSource -Credential 10
```

Retrieve CredentialInputSources associated with the Credential of ID 10.

## PARAMETERS

### -All
Retrieve resources from all pages.

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

### -Count
Number to retrieve per page.

```yaml
Type: UInt16
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: 20
Accept pipeline input: False
Accept wildcard characters: False
```

### -Credential
Credential ID or it's object.
Retrieve CredenialInputSource which the Credential associated with.

> [!TIP]  
> Can specify `IResource` object.  
> For example: `-Credential (Get-Credential-Id 3)`, `-Credential $creds[1]`

```yaml
Type: UInt64
Parameter Sets: (All)
Aliases:

Required: False
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Filter
Filtering various fields.

For examples:  
- `name__icontains=test`: "name" field contains "test" (case-insensitive).  
- `"name_ in=test,demo", created _gt=2024-01-01`: "name" field is "test" or "demo" and created after 2024-01-01.  
- `@{ Name = "name"; Value = "test"; Type = "Contains"; Not = $true }`: "name" field NOT contains "test"

For more details, see [about_AWX.psm_Filter_parameter](about_AWX.psm_Filter_parameter.md).

```yaml
Type: NameValueCollection
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -OrderBy
Retrieve list in the specified orders.
Use `!` prefix to sort in reverse.
Multiple sorting fields are available by separating with a comma(`,`).

Default value: `id` (ascending order of ID)

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: ["id"]
Accept pipeline input: False
Accept wildcard characters: False
```

### -Page
Page number.

```yaml
Type: UInt32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: 1
Accept pipeline input: False
Accept wildcard characters: False
```

### -Search
Search words. (case-insensitive)

Target fields: `description`

Multiple words are available by separating with a comma(`,`).

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

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
Credential ID or it's object.
See: `-Credential` parameter.

## OUTPUTS

### AWX.Resources.CredentialInputSource
## NOTES

## RELATED LINKS

[Get-CredentialInputSource](Get-CredentialInputSource.md)

[Get-Credential](Get-Credential.md)

[Find-Credential](Find-Credential.md)
