---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-AnsibleDashboard

## SYNOPSIS
Retrieve Dashboard.

## SYNTAX

```
Get-AnsibleDashboard [<CommonParameters>]
```

## DESCRIPTION
Retrieve AWX/AnsibleTower Dashboard data.

Implements following Rest API:  
- `/api/v2/dashboard/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-AnsibleDashboard

Inventories      : Total TotalWithInventorySource JobFailed InventoryFailed
                   ----- ------------------------ --------- ---------------
                       4                        1         0               0
InventorySources : Label      Total Failed
                   -----      ----- ------
                   Amazon EC2     0      0
Groups           : Total InventoryFailed
                   ----- ---------------
                       5               0
Hosts            : Total Failed
                   ----- ------
                       4      0
Projects         : Total Failed
                   ----- ------
                       3      0
ScmTypes         : Label          Total Failed
                   -----          ----- ------
                   Git                2      0
                   Subversion         0      0
                   Remote Archive     0      0
Users            : Total
                   -----
                       3
Organizations    : Total
                   -----
                       2
Teams            : Total
                   -----
                       1
JobTemplates     : Total
                   -----
                       6
```

Show Dashboard

## PARAMETERS

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None
## OUTPUTS

### Jagabata.Resources.Dashboard
## NOTES

## RELATED LINKS
