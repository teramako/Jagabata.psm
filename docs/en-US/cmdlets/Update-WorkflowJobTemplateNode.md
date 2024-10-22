---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Update-WorkflowJobTemplateNode

## SYNOPSIS
Update a WorkflowJobTemplateNode.

## SYNTAX

```
Update-WorkflowJobTemplateNode [-Id] <UInt64> [-UnifiedJobTemplate <UInt64>] [-ExtraData <String>]
 [-Inventory <UInt64>] [-ScmBranch <String>] [-JobType <String>] [-Tags <String>] [-SkipTags <String>]
 [-Limit <String>] [-DiffMode <Boolean>] [-Verbosity <JobVerbosity>] [-ExecutionEnvironment <UInt64>]
 [-Forks <Int32>] [-JobSliceCount <Int32>] [-Timeout <Int32>] [-AllParentsMustConverge <Boolean>]
 [-Identifier <String>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Update a WorkflowJobTemplateNode.

Implements following Rest API:  
- `/api/v2/workflow_job_template_nodes/{id}/` (PATCH)  

## EXAMPLES

### Example 1
```powershell
PS C:\> Update-WorkflowJobTemplateNode -Id 30 -ExtraData @{ message = "Hello World" }
```

## PARAMETERS

### -AllParentsMustConverge
If enabled then the node will only run if all of the parent nodes have met the criteria to reach this node.

```yaml
Type: Boolean
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DiffMode
Diff mode

```yaml
Type: Boolean
Parameter Sets: (All)
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
Parameter Sets: (All)
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
Parameter Sets: (All)
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
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
WorkflwJobTemplateNode ID to be updated.

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
Parameter Sets: (All)
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
Parameter Sets: (All)
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
Parameter Sets: (All)
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
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ScmBranch
Branch to use in job run.

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

### -SkipTags
Skip tags. (commas `,` separated)

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

### -Tags
Job tags. (commas `,` separated)

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
Parameter Sets: (All)
Aliases: Template

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Verbosity
Job verbosity.

```yaml
Type: JobVerbosity
Parameter Sets: (All)
Aliases:
Accepted values: Normal, Verbose, MoreVerbose, Debug, ConnectionDebug, WinRMDebug

Required: False
Position: Named
Default value: None
Accept pipeline input: False
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

### System.UInt64
WorkflowJobTemplateNode ID.

## OUTPUTS

### Jagabata.Resources.WorkflowJobTemplateNode
Updated WorkflowJobTemplateNode object.

## NOTES

## RELATED LINKS

[Get-WorkflowJobTemplateNode](Get-WorkflowJobTemplateNode.md)

[Find-WorkflowJobTemplateNode](Find-WorkflowJobTemplateNode.md)

[New-WorkflowJobTemplateNode](New-WorkflowJobTemplateNode.md)

[Remove-WorkflowJobTemplateNode](Remove-WorkflowJobTemplateNode.md)

[Register-WorkflowJobTemplateNode](Register-WorkflowJobTemplateNode.md)

[Unregister-WorkflowJobTemplateNode](Unregister-WorkflowJobTemplateNode.md)
