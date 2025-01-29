---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-AnsibleWorkflowJobNode

## SYNOPSIS
Retrieve nodes for WorkflowJob by ID(s).

## SYNTAX

```
Get-AnsibleWorkflowJobNode [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve nodes for WorkflowJob by specified ID(s).

Implementation of following API:  
- `/api/v2/workflow_job_nodes/{id}/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-AnsibleWorkflowJobNode -Id 10

Id            Type WorkflowJob       DoNotRun Modified           Job JobType JobName  JobStatus JobElapsed SuccessNodes FailureNodes AlwaysNodes
--            ---- -----------       -------- --------           --- ------- -------  --------- ---------- ------------ ------------ -----------
10 WorkflowJobNode [151]TestWorkflow    False 2024/07/11 9:27:45 152     Job Test_1  Successful      2.033 {}           {}           {11}
```

Retrieve a node in WorkflowJob for Database ID 1.

## PARAMETERS

### -Id
List of database IDs for one or more nodes in WorkflowJob.

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
One or more database IDs.

## OUTPUTS

### Jagabata.Resources.WorkflowJobNode
## NOTES

## RELATED LINKS

[Find-AnsibleWorkflowJobNode](Find-AnsibleWorkflowJobNode.md)
