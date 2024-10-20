---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-InventoryFile

## SYNOPSIS
Retrieve inventory files.

## SYNTAX

```
Get-InventoryFile [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve inventory files and directories available within this project, not comprehensive.

Implements following Rest API:  
- `/api/v2/projects/{id}/inventories/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-InventoryFile -Id 3
inventory/hosts.ini
inventory/hosts_2.ini
```

Retrieve inventory files within the Project in ID 3.

### Example 2
```powershell
PS C:\> Get-Project -Id 3 | Get-InventoryFile
inventory/hosts.ini
inventory/hosts_2.ini
```

Retrieve inventory files from pipeline inputed Project object.

## PARAMETERS

### -Id
List of database IDs for one or more Projects.

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

### System.String
Path string for inventory files and directories.

## NOTES

## RELATED LINKS

[Get-Project](Get-Project.md)

[Find-Project](Find-Project.md)
