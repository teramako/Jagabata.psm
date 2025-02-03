---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Invoke-AnsibleJobTemplate

## SYNOPSIS
Invoke (launch) a JobTemplate and wait unti the job is finished.

## SYNTAX

```
Invoke-AnsibleJobTemplate [-Id] <UInt64> [-Inventory <UInt64>] [-JobType <JobType>] [-ScmBranch <String>]
 [-Credentials <UInt64[]>] [-Limit <String>] [-Labels <UInt64[]>] [-Tags <String[]>] [-SkipTags <String[]>]
 [-ExtraVars <String>] [-DiffMode <Boolean>] [-Verbosity <JobVerbosity>] [-Forks <Int32>]
 [-ExecutionEnvironment <UInt64>] [-JobSliceCount <Int32>] [-Timeout <Int32>] [-Interactive]
 [-IntervalSeconds <Int32>] [-SuppressJobLog] [<CommonParameters>]
```

## DESCRIPTION
Launch the specified JobTemplate and wait until the job is finished.

Implementation of following API:  
- `/api/v2/job_templates/{id}/launch/`

## EXAMPLES

### Example 1
```powershell
PS C:\> > Invoke-AnsibleJobtemplate -Id 7
[7] Demo Job Template -
             Inventory : [1] Demo Inventory
            Extra vars : ---
             Diff Mode : False
              Job Type : Run
             Verbosity : 0 (Normal)
           Credentials : [1] Demo Credential
                 Forks : 0
       Job Slice Count : 1
               Timeout : 0
====== [100] Demo Job Template ======

PLAY [Hello World Sample] ******************************************************

TASK [Gathering Facts] *********************************************************
ok: [localhost]

TASK [Hello Message] ***********************************************************
ok: [localhost] => {
    "msg": "Hello World!"
}

PLAY RECAP *********************************************************************
localhost                  : ok=2    changed=0    unreachable=0    failed=0    skipped=0    rescued=0    ignored=0

 Id Type Name              JobType LaunchType     Status Finished            Elapsed LaunchedBy     Template             Note
 -- ---- ----              ------- ----------     ------ --------            ------- ----------     --------             ----
100 Job Demo Job Template     Run     Manual Successful 2024/08/06 15:19:01   1.983 [user][1]admin [7]Demo Job Template {[Playbook, hello_world.yml], [Artifacts, {}], [Labels, ]}
```

Launch JobTemplate ID 7, and wait unti for the job is finished.

## PARAMETERS

### -Credentials
Specify credential IDs or it's resource objects.

> [!NOTE]  
> This parameter will be ignored if "Ask" flag is off, although the request will be sent.

> [!TIP]  
> Can specify the resource as string like `Credential:1` (Format: `{Type}:{Id}`).
> And also accept objects have `type` and `id` properties.  
>
> For example:  
>  - `-Credentials (Get-AnsibleCredential -Id 1)`  
>  - `-Credentials @{ type = "credential"; id = 1 }`  
>  - `-Credentials credential:1`

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

### -DiffMode
Specify diff mode.

> [!NOTE]  
> This parameter will be ignored if "Ask" flag is off, although the request will be sent.

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
Specify ExecutionEnvironment ID or it's resource object.

> [!NOTE]  
> This parameter will be ignored if "Ask" flag is off, although the request will be sent.

> [!TIP]  
> Can specify the resource as string like `ExecutionEnvironment:1` (Format: `{Type}:{Id}`).
> And also accept objects have `type` and `id` properties.  
>
> For example:  
>  - `-ExecutionEnvironment (Get-AnsibleExecutionEnvironment -Id 1)`  
>  - `-ExecutionEnvironment Credentials @{ type = "executionEnvironment"; id = 1 }`  
>  - `-ExecutionEnvironment Credentials executionEnvironment:1`

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

### -Forks
Specify the number of forks.

> [!NOTE]  
> This parameter will be ignored if "Ask" flag is off, although the request will be sent.

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
JobTemplate ID or it's resource object to be launched.

> [!TIP]  
> Can specify the resource as string like `JobTemplate:1` (Format: `{Type}:{Id}`).
> And also accept objects have `type` and `id` properties.  
>
> For example:  
>  - `-Id (Get-AnsibleJobTemplate -Id 1)`  
>  - `-Id @{ type = "jobtemplate"; id = 1 }`  
>  - `-Id jobtemplate:1`

```yaml
Type: UInt64
Parameter Sets: (All)
Aliases: jobTemplate, jt

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

### -JobSliceCount
Specify the number of job slice count.

> [!NOTE]  
> This parameter will be ignored if "Ask" flag is off, although the request will be sent.

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

> [!NOTE]  
> This parameter will be ignored if "Ask" flag is off, although the request will be sent.

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

### -SuppressJobLog
Suppress display job log.

> [!TIP]  
> If you need the job log, use `-InformationVariable` parameter likes following:  
>  
>     PS C:\> Invoke-AnsibleJobTemplate ... -SuppressJobLog -InformationVariable joblog  
>     (snip)  
>     PS C:\> $joblog  
>     ====== [100] Demo Job Template ======  
>     
>     PLAY [Hello World Sample] ******************************************************  
>     
>     (snip)  
>     
>     PLAY RECAP *********************************************************************  
>     localhost                  : ok=2    changed=0    unreachable=0    failed=0    skipped=0    rescued=0    ignored=0

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

### -Timeout
Specify timeout value (seconds).

> [!NOTE]  
> This parameter will be ignored if "Ask" flag is off, although the request will be sent.

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

### -Verbosity
Specify job verbosity.

> [!NOTE]  
> This parameter will be ignored if "Ask" flag is off, although the request will be sent.

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.UInt64
JobTemplate ID or it's resource to be launched.
See: `-Id` parameter.

## OUTPUTS

### Jagabata.Resources.JobTemplateJob
The result job object of lanched the JobTemplate.

## NOTES

## RELATED LINKS

[Start-AnsibleJobTemplate](Start-AnsibleJobTemplate.md)

[Get-AnsibleJobTemplate](Get-AnsibleJobTemplate.md)

[Find-AnsibleJobTemplate](Find-AnsibleJobTemplate.md)
