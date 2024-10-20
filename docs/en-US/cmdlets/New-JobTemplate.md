---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# New-JobTemplate

## SYNOPSIS
Create a JobTemplate.

## SYNTAX

```
New-JobTemplate -Name <String> [-Description <String>] [-JobType <JobType>] [-Inventory <UInt64>]
 [-Project <UInt64>] -Playbook <String> [-ScmBranch <String>] [-Forks <Int32>] [-Limit <String>]
 [-Verbosity <JobVerbosity>] [-ExtraVars <String>] [-Tags <String>] [-ForceHandlers] [-SkipTags <String>]
 [-StartAtTask <String>] [-Timeout <Int32>] [-UseFactCache] [-ExecutionEnvironment <UInt64>]
 [-HostConfigKey <String>] [-AskScmBranch] [-AskDiffMode] [-AskVariables] [-AskLimit] [-AskTags] [-AskSkipTags]
 [-AskJobType] [-AskVerbosity] [-AskInventory] [-AskCredential] [-AskExecutionEnvironment] [-AskLabels]
 [-AskForks] [-AskJobSliceCount] [-SurveyEnabled] [-BecomeEnabled] [-DiffMode] [-AllowSimultaneous]
 [-JobSliceCount <Int32>] [-WebhookService <String>] [-WebhookCredential <UInt64>]
 [-PreventInstanceGroupFallback] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Create a JobTemplate.

Implementation of following API:  
- `/api/v2/job_templates/` (POST)

## EXAMPLES

### Example 1
```powershell
PS C:\> $inventory = Get-Inventory -Id 2
PS C:\> $project = Get-Project -Id 3
PS C:\> New-JobTemplate -Name jobTemplateName1 -Inventory $inventory -Project $project -Playbook path/to/playbok1.yml
```

## PARAMETERS

### -AllowSimultaneous

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

### -AskCredential
Ask credential on launch.

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

### -AskDiffMode
Ask diff-mode on launch.

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

### -AskExecutionEnvironment
Ask execution environment on launch.

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

### -AskForks
Ask number of forks on launch.

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

### -AskInventory
Ask inventory on launch.

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

### -AskJobSliceCount
Ask job slice count on launch.

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

### -AskJobType
Ask job-type on launch.

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

### -AskLabels
Ask labels on launch.

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

### -AskLimit
Ask limit on launch.

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

### -AskScmBranch
Ask scm-branch on launch.

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

### -AskSkipTags
Ask skip tags on launch.

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

### -AskTags
Ask job tags on launch.

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

### -AskVariables
Ask extra variables on launch.

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

### -AskVerbosity
Ask verbosity on launch.

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

### -BecomeEnabled

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

### -Description
Optional description of the JobTemplate.

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

### -DiffMode

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

### -ForceHandlers

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

### -HostConfigKey

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
Specify JobType ("Run" or "Check")

```yaml
Type: JobType
Parameter Sets: (All)
Aliases:
Accepted values: Run, Check

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
Name of the JobTemplate.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Playbook
File path of the playbook in the specified Project.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PreventInstanceGroupFallback
If enabled, the job template will prevent adding any inventory or organization instance groups to the list of preferred instances groups to run on.
If this setting is enabled and you provided an empty list, the global instance groups will be applied.

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

### -Project
Project ID.

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
Specify branch to use in job run. Project default is used if omitted.

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

### -StartAtTask

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
Type: SwitchParameter
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

### -UseFactCache
If enabled, the service will act as an Ansible Fact Cache Plugin; persisting facts at the end of a playbook run to the database and caching facts for use by Ansible.

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
Accepted values: github, gitlab, 

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

### None
## OUTPUTS

### Jagabata.Resources.JobTemplate
New created JobTemplate object.

## NOTES

## RELATED LINKS

[Get-JobTemplate](Get-JobTemplate.md)

[Find-JobTemplate](Find-JobTemplate.md)

[Update-Jobtemplate](Update-JobTemplate.md)

[Remove-JobTemplate](Remove-JobTemplate.md)
