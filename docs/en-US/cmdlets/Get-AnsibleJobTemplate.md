---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-AnsibleJobTemplate

## SYNOPSIS
Retrieve JobTemplates by the ID(s).

## SYNTAX

```
Get-AnsibleJobTemplate [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve JobTemplates by the specified ID(s).

Implements following Rest API:  
- `/api/v2/job_template/{id}/`  

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-AnsibleJobTemplate -Id 1
```

Retrieve a JobTemplate for Database ID 1.

## PARAMETERS

### -Id
List of database IDs for one or more JobTemplates.

```yaml
Type: UInt64[]
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.UInt64[]
List of database IDs for one or more JobTemplates.

## OUTPUTS

### Jagabata.Resources.JobTemplate
## NOTES

## RELATED LINKS

[Find-AnsibleJobTemplate](Find-AnsibleJobTemplate.md)

[New-AnsibleJobtemplate](New-AnsibleJobTemplate.md)

[Update-AnsibleJobTemplate](Update-AnsibleJobTemplate.md)

[Remove-AnsibleJobTemplate](Remove-AnsibleJobTemplate.md)

[Invoke-AnsibleJobTemplate](Invoke-AnsibleJobTemplate.md)

[Start-AnsibleJobTemplate](Start-AnsibleJobTemplate.md)
