---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-Playbook

## SYNOPSIS
Retrieve playbooks.

## SYNTAX

```
Get-Playbook [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve playbooks available associated with the Project.

Implements following Rest API:  
- `/api/v2/projects/{id}/playbooks/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-Playbook -Id 3
hello_world.yml
playbooks/demo_1.yml
playbooks/demo_2.yml
playbooks/demo_3.yml
```

Retrieve playbooks associated with the Project in ID 3.

### Example 2
```powershell
PS C:\> Get-Project -Id 3 | Get-Playbook
hello_world.yml
playbooks/demo_1.yml
playbooks/demo_2.yml
playbooks/demo_3.yml
```

Retrieve playbooks from pipeline inputed Project object.

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
Path string for playbook.

## NOTES

## RELATED LINKS

[Get-Project](Get-Project.md)

[Find-Project](Find-Project.md)
