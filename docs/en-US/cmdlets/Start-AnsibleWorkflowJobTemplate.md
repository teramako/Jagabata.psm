---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Start-AnsibleWorkflowJobTemplate

## SYNOPSIS
Invoke (update) a WorkflowJobTemplate.

## SYNTAX

```
Start-AnsibleWorkflowJobTemplate [-Id] <UInt64> [-Limit <String>] [-Inventory <UInt64>] [-ScmBranch <String>]
 [-Labels <UInt64[]>] [-Tags <String[]>] [-SkipTags <String[]>] [-ExtraVars <String>] [-Interactive]
 [<CommonParameters>]
```

## DESCRIPTION
Launch a WorkflowJobTemplate.
Multiple InventorySources in the Inventory may be udpated, when an Inventory is specified bye `-Inventory` parameter.

This command only sends a request to start WorkflowJobTemplate, not wait for the job is completed.
So, the returned job object will be non-completed status.
Use `Wait-AnsibleUnifiedJob` command to wait for the job to complete later.

Implementation of following API:  
- `/api/v2/workflow_job_templates/{id}/launch/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Start-AnsibleWorkflowJobTemplate -Id 13

 Id        Type Name         JobType LaunchType  Status Finished            Elapsed LaunchedBy     Template         Note
 --        ---- ----         ------- ----------  ------ --------            ------- ----------     --------         ----
110 WorkflowJob TestWorkflow             Manual Pending 2024/08/06 16:21:10   4.202 [user][1]admin [13]TestWorkflow {[Labels, test], [Inventory, [2]], [Limit, ], [Branch, ]…}
```

Launch WorkflowJobTemplate ID 13.

## PARAMETERS

### -ExtraVars
Specify extra variables.

Specify in JSON or YAML format.
You can also specify an object of type `IDictionary` as a parameter value.

Example: `-ExtraVars @{ key1 = "string"; key2 = 10; key3 = Get-Date }`

> [!NOTE]  
> This parameter will be ignored if "Ask" flag is off, although the request will be sent.

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

### -Id
WorkflowJobTemplate ID or it's resource object to be launched.

> [!TIP]  
> Can specify the resource as string like `WorkflowJobTemplate:1` (Format: `{Type}:{Id}`).
> And also accept objects have `type` and `id` properties.  
>
> For example:  
>  - `-Id (Get-AnsibleWorkflowJobTemplate -Id 1)`  
>  - `-Id @{ type = "workflowjobtemplate"; id = 1 }`  
>  - `-Id workflowjobtemplate:1`

```yaml
Type: UInt64
Parameter Sets: (All)
Aliases: workflowJobTemplate, wjt

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Interactive
"Prompt on launch" prompts for checked items.
(However, it will not prompt for items that have already been given as parameters.)

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

### -Inventory
Inventory ID or it's resource object.

> [!NOTE]  
> This parameter will be ignored if "Ask" flag is off, although the request will be sent.

> [!TIP]  
> Can specify the resource as string like `Inventory:1` (Format: `{Type}:{Id}`).
> And also accept objects have `type` and `id` properties.  
>
> For example:  
>  - `-Inventory (Get-AnsibleInventory -Id 1)`  
>  - `-Inventory @{ type = "inventory"; id = 1 }`  
>  - `-Inventory inventory:1`

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

### -Labels
Label IDs

> [!NOTE]  
> This parameter will be ignored if "Ask" flag is off, although the request will be sent.

```yaml
Type: UInt64[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Limit
Further limit selected hosts to an additional pattern.

> [!NOTE]  
> This parameter will be ignored if "Ask" flag is off, although the request will be sent.

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
Specify branch to use in job run. Project default is used if omitted.

> [!NOTE]  
> This parameter will be ignored if the Project's `AllowOverride` flag is on and  "Ask" flag is off, although the request will be sent.

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
Specify skip tags. (commas `,` separated)

> [!NOTE]  
> This parameter will be ignored if "Ask" flag is off, although the request will be sent.

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Tags
Specify tags. (commas `,` separated)

> [!NOTE]  
> This parameter will be ignored if "Ask" flag is off, although the request will be sent.

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

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
WorkflowJobTemplate ID or it's resource to be launched.
See: `-Id` parameter.

## OUTPUTS

### Jagabata.Resources.WorkflowJob+LaunchResult
The result job object of lanched the WorkflowJobTemplate (non-completed status).

## NOTES

## RELATED LINKS

[Invoke-AnsibleWorkflowJobTemplate](Invoke-AnsibleWorkflowJobTemplate.md)

[Get-AnsibleWorkflowJob](Get-AnsibleWorkflowJob.md)

[Find-AnsibleWorkflowJob](Find-AnsibleWorkflowJob.md)

[Wait-AnsibleUnifiedJob](Wait-AnsibleUnifiedJob.md)
