---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-AnsibleWorkflowJob

## SYNOPSIS
Retrieve WorkflowJob details by ID(s).

## SYNTAX

```
Get-AnsibleWorkflowJob [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve WorkflowJob details by the specified ID(s).

Implements following Rest API:  
- `/api/v2/workflow_jobs/{id}/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-AnsibleWorkflowJob -Id 20

Id        Type Name         JobType LaunchType     Status Finished            Elapsed LaunchedBy     Template         Note
--        ---- ----         ------- ----------     ------ --------            ------- ----------     --------         ----
20 WorkflowJob TestWorkflow             Manual Successful 2024/07/22 12:53:23   4.276 [user][1]admin [13]TestWorkflow {[Labels, test], [Inventory, [2]], [Limit, ], [Branch, ]…}
```

Retrieve a WorkflowJob detail for Database ID 1.

## PARAMETERS

### -Id
List of database IDs for one or more WorkflowJobs.

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
List of database IDs for one or more WorkflowJobs.

## OUTPUTS

### Jagabata.Resources.WorkflowJob+Detail
## NOTES

## RELATED LINKS

[Find-AnsibleWorkflowJob](Find-AnsibleWorkflowJob.md)

[Remove-AnsibleWorkflowJob](Remove-AnsibleWorkflowJob.md)
