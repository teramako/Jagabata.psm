---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Stop-AnsibleUnifiedJob

## SYNOPSIS
Stop (cancel) a running job.

## SYNTAX

### AssociatedWith (Default)
```
Stop-AnsibleUnifiedJob [-Type] <ResourceType> [-Id] <UInt64> [-Determine] [<CommonParameters>]
```

### PipelineInput
```
Stop-AnsibleUnifiedJob [-Job] <IResource> [-Determine] [<CommonParameters>]
```

## DESCRIPTION
Stop a running job.
This command only sends a request to cancel a job. AWX/AnsibleTower may response an error if not acceptable.

Can determine whether the job is cancelable with `-Determine` parameter.

Implementation of following API:  
- `/api/v2/jobs/{id}/cancel/`  
- `/api/v2/project_updates/{id}/cancel/`  
- `/api/v2/inventory_updates/{id}/cancel/`  
- `/api/v2/system_jobs/{id}/cancel/`  
- `/api/v2/ad_hoc_commands/{id}/cancel/`  
- `/api/v2/workflow_jobs/{id}/cancel/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Stop-AnsibleUnifiedJob -Type Job -Id 110

 Id Type   Status
 -- ----   ------
110  Job Accepted

PS C:\> Get-AnsibleJob -Id 110

 Id Type Name              JobType LaunchType   Status Finished  Elapsed LaunchedBy  Template Note
 -- ---- ----              ------- ----------   ------ --------  ------- ----------  -------- ----
110  Job Demo Job Template     Run     Manual Canceled ...           ... ...         ...      ...
```

Cancel Job ID 110.

### Example 2
```powershell
PS C:\> Stop-AnsibleUnifiedJob -Type Job -Id 110
Stop-AnsibleUnifiedJob: 405 (Method Not Allowed): {"detail":"Method \"POST\" not allowed."} on POST /api/v2/jobs/110/cancel/

 Id Type           Status
 -- ----           ------
110  Job MethodNotAllowed
```

An error sample, when trying to cancel the job ID 110 that has already been completed.

### Example 3
```powershell
PS C:\> Stop-AnsibleUnifiedJob -Type Job -Id 110 -Determine

 Id Type CanCancel
 -- ---- ---------
110  Job     False
```

Check whether the Job ID 100 is canelable.

## PARAMETERS

### -Determine
Determine whether the job is canelable, instead of requesting cancel.

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

### -Job
UnifiedJob resource object from which to stop.

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

### Jagabata.IResource
The object has `Id` and `Type` properties.

And `Type` should be following value:  
- `Job`  
- `ProjectUpdate`  
- `InventoryUpdate`  
- `SystemJob`  
- `AdHocCommand`  
- `WorkflowJob`

## OUTPUTS

### System.Management.Automation.PSObject
The results of requested to cancel or determine whether cancelable.

## NOTES

## RELATED LINKS

[Start-AnsibleJobTemplateJob](Start-AnsibleJobTemplate.md)

[Start-AnsibleProjectUpdate](Start-AnsibleProjectUpdate.md)

[Start-AnsibleInventoryUpdate](Start-AnsibleInventoryUpdate.md)

[Start-AnsibleSystemJobTemplate](Start-AnsibleSystemJobTemplate.md)

[Start-AnsibleAdHocCommand](Start-AnsibleAdHocCommand.md)

[Start-AnsibleWorkflowJobTemplate](Start-AnsibleWorkflowJobTemplate.md)

[Find-AnsibleUnifiedJob](Find-AnsibleUnifiedJob.md)

[Wait-AnsibleUnifiedJob](Wait-AnsibleUnifiedJob.md)
