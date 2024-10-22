---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-ActivityStream

## SYNOPSIS
Retrieves ActivityStreams by ID(s).

## SYNTAX

```
Get-ActivityStream [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retreive ActivityStreams by the specified ID.

Implements following Rest API:  
- `/api/v2/activity_stream/{id}/`  

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-ActivityStream -Id 1
```

Retrieve an ActivityStream for Database ID 1.

### Example 2
```powershell
PS C:\> 1..4 | Get-ActivityStream
```

Retrieve ActivityStreams with ID numbers 1 though 4.

## PARAMETERS

### -Id
List of database IDs for one or more ActivityStreams.

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

### Jagabata.Resources.ActivityStream
## NOTES

## RELATED LINKS

[Find-ActivityStream](Find-ActivityStream.md)
