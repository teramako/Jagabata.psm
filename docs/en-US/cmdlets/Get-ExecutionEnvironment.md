---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-ExecutionEnvironment

## SYNOPSIS
Retrieve ExecutionEnvironments by the ID(s).

## SYNTAX

```
Get-ExecutionEnvironment [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve ExecutionEnvironments by the specified ID(s).

Implements following Rest API:  
- `/api/v2/execution_environments/{id}/`  

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-ExecutionEnvironment -Id 1
```

Retrieve an ExecutionEnvironment for Database ID 1.

## PARAMETERS

### -Id
List of database IDs for one or more ExecutionEnvironments.

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

### Jagabata.Resources.ExecutionEnvironment
## NOTES

## RELATED LINKS

[Find-ExecutionEnvironment](Find-ExecutionEnvironment.md)

[New-ExecutionEnvironment](New-ExecutionEnvironment.md)

[Update-ExecutionEnvironment](Update-ExecutionEnvironment.md)

[Remove-ExecutionEnvironment](Remove-ExecutionEnvironment.md)
