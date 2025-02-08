---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-AnsibleHostMetric

## SYNOPSIS
Retrieve HostMetrics by the ID(s).

## SYNTAX

```
Get-AnsibleHostMetric [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve HostMetrics by the specified ID(s).

Implements following Rest API:  
- `/api/v2/host_metrics/{id}/`  

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-AnsibleHostMetric -Id 1
```

Retrieve a HostMetric for Database ID 1.

## PARAMETERS

### -Id
List of database IDs for one or more HostMetrics.

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
List of database IDs for one or more HostMetrics.

## OUTPUTS

### Jagabata.Resources.HostMetric

## NOTES

## RELATED LINKS

[Find-AnsibleHostMetric](Find-AnsibleHostMetric.md)
