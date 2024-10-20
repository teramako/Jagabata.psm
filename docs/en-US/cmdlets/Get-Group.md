---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-Group

## SYNOPSIS
Retrieve Groups by the ID(s).

## SYNTAX

```
Get-Group [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve Groups by the specified ID(s).

Implements following Rest API:  
- `/api/v2/groups/{id}/`  

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-Group -Id 1
```

Retrieve a Group for Database ID 1.

## PARAMETERS

### -Id
List of database IDs for one or more Groups.

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

### Jagabata.Resources.Group
## NOTES

## RELATED LINKS

[Find-Group](Find-Group.md)

[New-Group](New-Group.md)

[Update-Group](Update-Group.md)

[Register-Group](Register-Group.md)

[Unregister-Group](Unregister-Group.md)

[Remove-Group](Remove-Group.md)
