---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# New-AnsibleWorkflowJobTemplateNode

## SYNOPSIS
Create a WorkflowJobTemplateNode.

## SYNTAX

### UnifiedJobTemplate (Default)
```
New-AnsibleWorkflowJobTemplateNode [-WorkflowJobtemplate] <UInt64> [[-ParentNode] <UInt64>]
 [[-RunUpon] <String>] [-UnifiedJobTemplate] <UInt64> [-ExtraData <String>] [-Inventory <UInt64>]
 [-ScmBranch <String>] [-JobType <String>] [-Tags <String>] [-SkipTags <String>] [-Limit <String>] [-DiffMode]
 [-Verbosity <JobVerbosity>] [-ExecutionEnvironment <UInt64>] [-Forks <Int32>] [-JobSliceCount <Int32>]
 [-Timeout <Int32>] [-AllParentsMustConverge] [-Identifier <String>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

### WorkflowApproval
```
New-AnsibleWorkflowJobTemplateNode [-WorkflowJobtemplate] <UInt64> [[-ParentNode] <UInt64>]
 [[-RunUpon] <String>] -ApprovalName <String> [-Description <String>] [-Timeout <Int32>]
 [-AllParentsMustConverge] [-Identifier <String>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Create a WorkflowJobTemplateNode into the WorkflowJobTemplate.

Implements following Rest API:  
- `/api/v2/workflow_job_templates/{id}/workflow_nodes/` (POST)  
- `/api/v2/workflow_job_template_nodes/{id}/success_nodes/` (POST)  
- `/api/v2/workflow_job_template_nodes/{id}/failure_nodes/` (POST)  
- `/api/v2/workflow_job_template_nodes/{id}/always_nodes/` (POST)  
- `/api/v2/workflow_job_template_nodes/{id}/create_approval_template/` (POST)

## EXAMPLES

### Example 1
```powershell
PS C:\> $wjt = Get-AnsibleWorkflowJobTemplate -Id 1
PS C:\> $jt = Get-AnsibleJobTemplate -Id 20
PS C:\> New-AnsibleWorkflowJobTemplateNode -WorkflowJobTemplate $wjt -UnifiedJobTemplate $jt
```

### Example 2
```powershell
PS C:\> Find-AnsibleWorkflowJobTemplateNode -Id 1 -OutVariables nodes
PS C:\> New-AnsibleWorkflowJobTemplateNode -WorkflowJobTemplate 1 -UnifiedJobTemplate 20 -ParentNode $nodes[-1] -RunUpon always
```

### Example 3
```powershell
PS C:\> New-AnsibleWorkflowJobTemplateNode -WorkflowJobTemplate 1 -ApprovalName Wait-AnsibleApproval-From-Boss -Timeout 3600
```

## PARAMETERS

### -AllParentsMustConverge
If enabled then the node will only run if all of the parent nodes have met the criteria to reach this node.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ApprovalName
Name of WorkflowApprovalTemplate.

```yaml
Type: String
Parameter Sets: WorkflowApproval
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Description
Optional description of the WorkflowApprovalTemplate.

```yaml
Type: String
Parameter Sets: WorkflowApproval
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DiffMode

```yaml
Type: SwitchParameter
Parameter Sets: UnifiedJobTemplate
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ExecutionEnvironment
ExecutionEnvironment ID.

```yaml
Type: UInt64
Parameter Sets: UnifiedJobTemplate
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ExtraData
Extra variables.

Specify in JSON or YAML format.
You can also specify an object of type `IDictionary` as a parameter value.

Example: `-ExtraVars @{ key1 = "string"; key2 = 10; key3 = Get-Date }`

```yaml
Type: String
Parameter Sets: UnifiedJobTemplate
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Forks
Number of forks.

```yaml
Type: Int32
Parameter Sets: UnifiedJobTemplate
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Identifier
An identifier for this node that is unique within its workflow.
It is copied to workflow job nodes corresponding to this node.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Inventory
Inventory ID.

```yaml
Type: UInt64
Parameter Sets: UnifiedJobTemplate
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -JobSliceCount
Number of job slice count.

```yaml
Type: Int32
Parameter Sets: UnifiedJobTemplate
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -JobType
Specify JobType ("run" or "check")

```yaml
Type: String
Parameter Sets: UnifiedJobTemplate
Aliases:
Accepted values: run, check, 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Limit
Further limit selected hosts to an additional pattern.

```yaml
Type: String
Parameter Sets: UnifiedJobTemplate
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ParentNode
WorkflowJobTemplateNode ID to be parent.

```yaml
Type: UInt64
Parameter Sets: (All)
Aliases:

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -RunUpon
Specifies which state ("success", "failure", or "always") the job on the parent node should run in.

```yaml
Type: String
Parameter Sets: (All)
Aliases: Upon
Accepted values: success, failure, always

Required: False
Position: 2
Default value: success
Accept pipeline input: False
Accept wildcard characters: False
```

### -ScmBranch
Branch to use in job run.

```yaml
Type: String
Parameter Sets: UnifiedJobTemplate
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SkipTags
Skip tags. (commas `,` separated)

```yaml
Type: String
Parameter Sets: UnifiedJobTemplate
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Tags
Job tags. (commas `,` separated)

```yaml
Type: String
Parameter Sets: UnifiedJobTemplate
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Timeout
Timeout value (seconds).

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -UnifiedJobTemplate
UnifiedJobTemplate (JobTemplate, Project, InventorySource, WorkflowJobTemplate) ID.

```yaml
Type: UInt64
Parameter Sets: UnifiedJobTemplate
Aliases: Template

Required: True
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Verbosity
Job verbosity.

```yaml
Type: JobVerbosity
Parameter Sets: UnifiedJobTemplate
Aliases:
Accepted values: Normal, Verbose, MoreVerbose, Debug, ConnectionDebug, WinRMDebug

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -WorkflowJobtemplate
WorkflowJobTemplate ID.

```yaml
Type: UInt64
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Confirm
Prompts you for confirmation before running the cmdlet.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: cf

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -WhatIf
Shows what would happen if the cmdlet runs.
The cmdlet is not run.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: wi

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None
## OUTPUTS

### Jagabata.Resources.WorkflowJobTemplateNode
New created WorkflowJobTemplateNode object.

## NOTES

## RELATED LINKS

[Get-AnsibleWorkflowJobTemplateNode](Get-AnsibleWorkflowJobTemplateNode.md)

[Find-AnsibleWorkflowJobTemplateNode](Find-AnsibleWorkflowJobTemplateNode.md)

[Update-AnsibleWorkflowJobTemplateNode](Update-AnsibleWorkflowJobTemplateNode.md)

[Remove-AnsibleWorkflowJobTemplateNode](Remove-AnsibleWorkflowJobTemplateNode.md)

[Register-AnsibleWorkflowJobTemplateNode](Register-AnsibleWorkflowJobTemplateNode.md)

[Unregister-AnsibleWorkflowJobTemplateNode](Unregister-AnsibleWorkflowJobTemplateNode.md)
