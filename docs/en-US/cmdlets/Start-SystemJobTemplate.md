---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Start-SystemJobTemplate

## SYNOPSIS
Invoke (launch) a SystemJobTemplate.

## SYNTAX

### Id
```
Start-SystemJobTemplate [-Id] <UInt64> [-ExtraVars <IDictionary>] [<CommonParameters>]
```

### Template
```
Start-SystemJobTemplate [-SystemJobTemplate] <IResource> [-ExtraVars <IDictionary>] [<CommonParameters>]
```

## DESCRIPTION
Launch a SystemJobTemplate.

This command only sends a request to start SystemJobTemplate, not wait for the job is completed.
So, the returned job object will be non-completed status.
Use `Wait-UnifiedJob` command to wait for the job to complete later.

Implementation of following API:  
- `/api/v2/system_job_templates/{id}/launch/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Start-SystemJobTemplate -Id 4
====== [110] Cleanup Expired Sessions ======
Expired Sessions deleted 2


 Id      Type Name                              JobType LaunchType  Status Finished            Elapsed LaunchedBy     Template                    Note
 --      ---- ----                              ------- ----------  ------ --------            ------- ----------     --------                    ----
110 SystemJob Cleanup Expired Sessions cleanup_sessions     Manual Pending 2024/08/06 15:56:27   1.793 [user][1]admin [4]Cleanup Expired Sessions {[ExtraVars, {}], [Stdout, Expired Sessions deleted 2…
```

Launch JobTemplate ID 4.

## PARAMETERS

### -ExtraVars
Variables to be passed to the system job task as command line parameters.

For excample:  
- `@{ dry_run: $true }` : for `cleanup_jobs` and `cleanup_activitystream`  
- `@{ days: 90 }'`      : for `cleanup_jobs` and `cleanup_activitystream`

```yaml
Type: IDictionary
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
SystemJobTemplate ID to be launched.

```yaml
Type: UInt64
Parameter Sets: Id
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -SystemJobTemplate
{{ Fill SystemJobTemplate Description }}

```yaml
Type: IResource
Parameter Sets: Template
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

### System.UInt64
SystemJobTemplate ID to be launched.

### Jagabata.Resources.SystemJobTemplate
SystemJobTemplate object to be launched.

## OUTPUTS

### Jagabata.Resources.SystemJob+Detail
The result job object of lanched the SystemJobTemplate (non-completed status).

## NOTES

## RELATED LINKS

[Invoke-SystemJobTemplate](Invoke-SystemJobTemplate.md)

[Get-SystemJob](Get-SystemJob.md)

[Find-SystemJob](Find-SystemJob)

[Wait-UnifiedJob](Wait-UnifiedJob.md)
