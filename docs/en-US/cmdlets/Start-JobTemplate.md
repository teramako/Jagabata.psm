---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Start-JobTemplate

## SYNOPSIS
Invoke (launch) a JobTemplate.

## SYNTAX

### Id
```
Start-JobTemplate [-Id] <UInt64> [-Inventory <UInt64>] [-JobType <JobType>] [-ScmBranch <String>]
 [-Credentials <UInt64[]>] [-Limit <String>] [-Labels <UInt64[]>] [-Tags <String[]>] [-SkipTags <String[]>]
 [-ExtraVars <String>] [-DiffMode <Boolean>] [-Verbosity <JobVerbosity>] [-Forks <Int32>]
 [-ExecutionEnvironment <UInt64>] [-JobSliceCount <Int32>] [-Timeout <Int32>] [-Interactive]
 [<CommonParameters>]
```

### JobTemplate
```
Start-JobTemplate [-JobTemplate] <IResource> [-Inventory <UInt64>] [-JobType <JobType>] [-ScmBranch <String>]
 [-Credentials <UInt64[]>] [-Limit <String>] [-Labels <UInt64[]>] [-Tags <String[]>] [-SkipTags <String[]>]
 [-ExtraVars <String>] [-DiffMode <Boolean>] [-Verbosity <JobVerbosity>] [-Forks <Int32>]
 [-ExecutionEnvironment <UInt64>] [-JobSliceCount <Int32>] [-Timeout <Int32>] [-Interactive]
 [<CommonParameters>]
```

## DESCRIPTION
Launch a JobTemplate.

This command only sends a request to start JobTemplate, not wait for the job is completed.
So, the returned job object will be non-completed status.
Use `Wait-UnifiedJob` command to wait for the job to complete later.

Implementation of following API:  
- `/api/v2/job_templates/{id}/launch/`

## EXAMPLES

### Example 1
```powershell
PS C:\> > Start-Jobtemplate -Id 7

 Id Type Name              JobType LaunchType  Status Finished            Elapsed LaunchedBy     Template             Note
 -- ---- ----              ------- ----------  ------ --------            ------- ----------     --------             ----
101 Job Demo Job Template     Run     Manual  Pending 2024/08/06 15:19:01   1.983 [user][1]admin [7]Demo Job Template {[Playbook, hello_world.yml], [Artifacts, {}], [Labels, ]}
```

Launch JobTemplate ID 7.

### Example 2
```powershell
PS C:\> > Start-Jobtemplate -Id 7 | Wait-UnifiedJob
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
====== [102] Demo Job Template ======

PLAY [Hello World Sample] ******************************************************

TASK [Gathering Facts] *********************************************************
ok: [localhost]

TASK [Hello Message] ***********************************************************
ok: [localhost] => {
    "msg": "Hello World!"
}

PLAY RECAP *********************************************************************
localhost                  : ok=2    changed=0    unreachable=0    failed=0    skipped=0    rescued=0    ignored=0

 Id Type Name              JobType LaunchType     Status Finished           Elapsed LaunchedBy     Template             Note
 -- ---- ----              ------- ----------     ------ --------           ------- ----------     --------             ----
102 Job Demo Job Template     Run     Manual Successful 2024/08/07 9:53:56   1.968 [user][1]admin [7]Demo Job Template {[Playbook, hello_world.yml], [Artifacts, {}], [Labels, ]}
```

Launch JobTemplate ID 7, and wait unti for the job is finished.
This is almost same as `Invoke-JobTemplate` command.

## PARAMETERS

### -Credentials
Specify credential IDs.

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
Specify ExecutionEnvironment ID.

> [!NOTE]  
> This parameter will be ignored if "Ask" flag is off, although the request will be sent.

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
JobTemplate ID to be launched.

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
Inventory ID

> [!NOTE]  
> This parameter will be ignored if "Ask" flag is off, although the request will be sent.

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

### -JobTemplate
JobTempalte object to be launched.

```yaml
Type: IResource
Parameter Sets: JobTemplate
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
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
JobTemplate ID to be launched.

### Jagabata.Resources.JobTemplate
JobTemplate object to be launched.

## OUTPUTS

### Jagabata.Resources.JobTemplateJob+LaunchResult
The result job object of lanched the JobTemplate (non-completed status)

## NOTES

## RELATED LINKS

[Invoke-JobTemplate](Invoke-JobTemplate.md)

[Get-JobTemplate](Get-JobTemplate.md)

[Find-JobTemplate](Find-JobTemplate.md)

[Wait-UnifiedJob](Wait-UnifiedJob.md)
