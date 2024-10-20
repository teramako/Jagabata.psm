---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-JobTemplate

## SYNOPSIS
Retrieve JobTemplates by the ID(s).

## SYNTAX

```
Get-JobTemplate [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve JobTemplates by the specified ID(s).

Implements following Rest API:  
- `/api/v2/job_template/{id}/`  

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-JobTemplate -Id 1
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
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.UInt64[]
One or more database IDs.

## OUTPUTS

### Jagabata.Resources.JobTemplate
## NOTES

## RELATED LINKS

[Find-JobTemplate](Find-JobTemplate.md)

[New-Jobtemplate](New-JobTemplate.md)

[Update-JobTemplate](Update-JobTemplate.md)

[Remove-JobTemplate](Remove-JobTemplate.md)

[Invoke-JobTemplate](Invoke-JobTemplate.md)

[Start-JobTemplate](Start-JobTemplate.md)
