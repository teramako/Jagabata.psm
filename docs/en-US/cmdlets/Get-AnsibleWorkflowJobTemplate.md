---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-AnsibleWorkflowJobTemplate

## SYNOPSIS
Retrieve WorkflowJobTemplates by the ID(s).

## SYNTAX

```
Get-AnsibleWorkflowJobTemplate [-Id] <UInt64[]> [<CommonParameters>]
```

## DESCRIPTION
Retrieve WorkflowJobTemplates by the specified ID(s).

Implements following Rest API:  
- `/api/v2/workflow_job_templates/{id}/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-AnsibleWorkflowJobTemplate -Id 13

Id                Type Name         Description         Status Modified            LastJobRun          NextJobRun          Options Note
--                ---- ----         -----------         ------ --------            ----------          ----------          ------- ----
13 WorkflowJobTemplate TestWorkflow Sample Workflow Successful 2024/07/10 18:45:06 2024/07/22 12:53:23 2024/06/12 17:45:00 None    {[Organization, [2]TestOrg], [Inventory, [2]TestInventory], [Branch, ], [Limit, ]…}
```

Retrieve a WorkflowJobTemplate for Database ID 13.

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

### Jagabata.Resources.WorkflowJobTemplate
## NOTES

## RELATED LINKS

[Find-AnsibleWorkflowJobTemplate](Find-AnsibleWorkflowJobTemplate.md)

[New-AnsibleWorkflowJobTemplate](New-AnsibleWorkflowJobTemplate.md)

[Update-AnsibleWorkflowJobTemplate](Update-AnsibleWorkflowJobTemplate.md)

[Remove-AnsibleWorkflowJobTemplate](Remove-AnsibleWorkflowJobTemplate.md)
