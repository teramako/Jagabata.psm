---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Update-WorkflowJobTemplate

## SYNOPSIS
Update a WorkflowJobTemplate.

## SYNTAX

```
Update-WorkflowJobTemplate [-Id] <UInt64> [-Name <String>] [-Description <String>] [-Organization <UInt64>]
 [-Inventory <UInt64>] [-Limit <String>] [-ScmBranch <String>] [-ExtraVars <String>] [-Tags <String>]
 [-SkipTags <String>] [-AskScmBranch <Boolean>] [-AskVariables <Boolean>] [-AskLimit <Boolean>]
 [-AskTags <Boolean>] [-AskSkipTags <Boolean>] [-AskInventory <Boolean>] [-AskLabels <Boolean>]
 [-SurveyEnabled <Boolean>] [-WebhookService <String>] [-WebhookCredential <UInt64>]
 [-AllowSimultaneous <Boolean>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Update a WorkflowJobTemplate.

Implementation of following API:  
- `/api/v2/workflow_job_templates/{id}/` (PATCH)

## EXAMPLES

### Example 1
```powershell
PS C:\> Update-WorkflowJobTemplate -Id 3 -AskVariables $true
```

## PARAMETERS

### -AllowSimultaneous

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

### -AskInventory
Ask inventory on launch.

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

### -AskLabels
Ask labels on launch.

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

### -AskLimit
Ask limit on launch.

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

### -AskScmBranch
Ask scm-branch on launch.

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

### -AskSkipTags
Ask skip tags on launch.

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

### -AskTags
Ask job tags on launch.

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

### -AskVariables
Ask extra variables on launch.

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

### -Description
Optional description of the WorkflowJobTemplate.

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

### -ExtraVars
Specify extra variables.

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

### -Id
WorkflowJobTemplate ID to be updated.

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

### -Name
Name of the WorkflowJobTemplate.

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

### -Organization
Organization ID.

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

### -ScmBranch
Specify branch to use in job run.

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

### -SurveyEnabled

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

### -WebhookCredential
Credential ID of Personal Access Token (`github` or `gitlab`) for posting back the status to the service api.

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

### -WebhookService
Service that webhook requests will be accepted from `github` or `gitlab`

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: github, gitlab

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
WorkflowJobTemplate ID.

## OUTPUTS

### Jagabata.Resources.WorkflowJobTemplate
Updated WorkflowJobTemplate object.

## NOTES

## RELATED LINKS

[Get-WorkflowJobTemplate](Get-WorkflowJobTemplate.md)

[Find-WorkflowJobTemplate](Find-WorkflowJobTemplate.md)

[New-WorkflowJobTemplate](New-WorkflowJobTemplate.md)

[Remove-WorkflowJobTemplate](Remove-WorkflowJobTemplate.md)
