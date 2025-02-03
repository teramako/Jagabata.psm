---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Invoke-AnsibleProjectUpdate

## SYNOPSIS
Invoke (update) a Project and wait until the job is finished.

## SYNTAX

### Update (Default)
```
Invoke-AnsibleProjectUpdate [-Id] <UInt64> [-IntervalSeconds <Int32>] [-SuppressJobLog] [<CommonParameters>]
```

### Check
```
Invoke-AnsibleProjectUpdate [-Id] <UInt64> -Check [<CommonParameters>]
```

## DESCRIPTION
Update a Project and wait until the job is finished.

Implementation of following API:  
- `/api/v2/projects/{id}/update/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Invoke-AnsibleProjectUpdate -Id 8
====== [100] proj_1 ======
Identity added: /runner/artifacts/336/ssh_key_data (***@********)

PLAY [Update source tree if necessary] *****************************************

(snip)

PLAY RECAP *********************************************************************
localhost                  : ok=3    changed=0    unreachable=0    failed=0    skipped=4    rescued=0    ignored=0

 Id          Type Name   JobType LaunchType     Status Finished            Elapsed LaunchedBy     Template       Note
 --          ---- ----   ------- ----------     ------ --------            ------- ----------     --------       ----
100 ProjectUpdate proj_1   Check     Manual Successful 2024/08/06 15:34:34   1.888 [user][1]admin [8][git]proj_1 {[Branch, master], [Revision, 3cc7efff0ab80a0108456317c47214509728c9d3], [Url, git@gitrepo:repo1.git]}
```

Update a Project ID 8, and wait until for the job is finished.

### Example 2
```powershell
PS C:\> Invoke-AnsibleProjectUpdate -Id 8 -Check

Id    Type CanUpdate
--    ---- ---------
 8 Project      True
```

Check wheter Project ID 8 can be updated.

## PARAMETERS

### -Check
Check wheter a Project can be updated.

```yaml
Type: SwitchParameter
Parameter Sets: Check
Aliases:

Required: True
Position: Named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
Project ID or it's resource object to be updated.

> [!TIP]  
> Can specify the resource as string like `Project:1` (Format: `{Type}:{Id}`).
> And also accept objects have `type` and `id` properties.  
>
> For example:  
>  - `-Id (Get-AnsibleProject -Id 1)`  
>  - `-Id @{ type = "project"; id = 1 }`  
>  - `-Id project:1`

```yaml
Type: UInt64
Parameter Sets: (All)
Aliases: project, p

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -IntervalSeconds
Interval to confirm job completion (seconds).
Default is 5 seconds.

```yaml
Type: Int32
Parameter Sets: Update
Aliases:

Required: False
Position: Named
Default value: 5
Accept pipeline input: False
Accept wildcard characters: False
```

### -SuppressJobLog
Suppress display job log.

> [!TIP]  
> If you need the job log, use `-InformationVariable` parameter likes following:  
>  
>     PS C:\> Invoke-AnsibleProjectUpdate ... -SuppressJobLog -InformationVariable joblog  
>     (snip)  
>     PS C:\> $joblog  
>     ====== [100] proj_1 ======  
>     
>     (snip)  
>     
>     PLAY RECAP *********************************************************************  
>     localhost                  : ok=3    changed=0    unreachable=0    failed=0    skipped=4    rescued=0    ignored=0

```yaml
Type: SwitchParameter
Parameter Sets: Update
Aliases:

Required: False
Position: Named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.UInt64
Project ID or it's resource to be updated.
See: `-Id` parameter.

## OUTPUTS

### Jagabata.Resources.ProjectUpdateJob
The result job object of updated the Project.

### System.Management.Automation.PSObject
Results of checked wheter the Project can be updated.

## NOTES

## RELATED LINKS

[Start-AnsibleProjectUpdate](Start-AnsibleProjectUpdate.md)

[Get-AnsibleProjectUpdate](Get-AnsibleProjectUpdate.md)

[Find-AnsibleProjectUpdate](Find-AnsibleProjectUpdate.md)
