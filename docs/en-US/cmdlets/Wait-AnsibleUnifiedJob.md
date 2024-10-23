---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Wait-AnsibleUnifiedJob

## SYNOPSIS
Wait until jobs are finished.

## SYNTAX

### AssociatedWith
```
Wait-AnsibleUnifiedJob [-Type] <ResourceType> [-Id] <UInt64> [-IntervalSeconds <Int32>] [-SuppressJobLog]
 [<CommonParameters>]
```

### PipelineInput
```
Wait-AnsibleUnifiedJob [-Job] <IResource> [-IntervalSeconds <Int32>] [-SuppressJobLog]
 [<CommonParameters>]
```

## DESCRIPTION
Wait until a job is finished.
While waiting, retrieve the logs periodically and ouput.

## EXAMPLES

### Example 1
```powershell
PS C:\> Wait-AnsibleUnifiedJob -Type Job -Id 110
====== [110] Demo Job Template ======

PLAY [Hello World Sample] ******************************************************

TASK [Gathering Facts] *********************************************************
ok: [localhost]

TASK [Hello Message] ***********************************************************
ok: [localhost] => {
    "msg": "Hello World!"
}

PLAY RECAP *********************************************************************
localhost                  : ok=2    changed=0    unreachable=0    failed=0    skipped=0    rescued=0    ignored=0

 Id Type Name              JobType LaunchType     Status Finished Elapsed LaunchedBy     Template Note
 -- ---- ----              ------- ----------     ------ -------- ------- ----------     -------- ----
110  Job Demo Job Template     Run     Manual Successful ....         ... ...             ...      ...
```

Wait until Job ID 110 is completed.

### Example 2
```powershell
PS C:\> Find-AnsibleJob -Status running | Wait-AnsibleUnifiedJob
```

Retrieve running jobs currently, and wait until those jobs are completed.

## PARAMETERS

### -Id
Job ID of the target resource.
Use in conjection with the `-Type` parameter.

```yaml
Type: UInt64
Parameter Sets: AssociatedWith
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -IntervalSeconds
Interval to confirm job completion (seconds).
Default is 5 seconds.

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: 5
Accept pipeline input: False
Accept wildcard characters: False
```

### -Job
UnifiedJob resource object from which to wait.

The resource is an object with `Id` and `Type` properties.
And `Type` should be following value:  
- `Job`             : JobTempalte's job  
- `ProjectUpdate`   : Project's update job  
- `InventoryUpdate` : InventorySource's update job  
- `AdHocCommand`    : AdHocCommand's job  
- `WorkflowJob`     : WorkflowJobTemplate's job  
- `SystemJob`       : SystemJobTemplate's job

```yaml
Type: IResource
Parameter Sets: PipelineInput
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -SuppressJobLog
Suppress display job log.

> [!TIP]  
> If you need the job log, use `-InformationVariable` parameter likes following:  
>  
>     PS C:\> Wait-AnsibleUnifiedJob ... -SuppressJobLog -InformationVariable joblog  
>     (snip)  
>     PS C:\> $joblog  
>     ====== [111] Test_1 ======  
>     
>     (snip)  

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### -Type
Resource type name of the target.
Use in conjection with the `-Id` parameter.

```yaml
Type: ResourceType
Parameter Sets: AssociatedWith
Aliases:
Accepted values: Job, ProjectUpdate, InventoryUpdate, SystemJob, AdHocCommand, WorkflowJob

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### Jagabata.Resources.IResource
The object has `Id` and `Type` properties.

And `Type` should be following value:  
- `Job`  
- `ProjectUpdate`  
- `InventoryUpdate`  
- `SystemJob`  
- `AdHocCommand`  
- `WorkflowJob`

## OUTPUTS

### Jagabata.Resources.JobTemplateJob
### Jagabata.Resources.ProjectUpdateJob
### Jagabata.Resources.InventoryUpdateJob
### Jagabata.Resources.SystemJob
### Jagabata.Resources.AdHocCommand
### Jagabata.Resources.WorkflowJob
## NOTES

## RELATED LINKS

[Start-AnsibleJobTemplateJob](Start-AnsibleJobTemplate.md)

[Start-AnsibleProjectUpdate](Start-AnsibleProjectUpdate.md)

[Start-AnsibleInventoryUpdate](Start-AnsibleInventoryUpdate.md)

[Start-AnsibleSystemJobTemplate](Start-AnsibleSystemJobTemplate.md)

[Start-AnsibleAdHocCommand](Start-AnsibleAdHocCommand.md)

[Start-AnsibleWorkflowJobTemplate](Start-AnsibleWorkflowJobTemplate.md)

[Stop-AnsibleUnifiedJob](Stop-AnsibleUnifiedJob.md)

[Find-AnsibleUnifiedJob](Find-AnsibleUnifiedJob.md)
