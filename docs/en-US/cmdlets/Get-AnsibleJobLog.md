---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Get-AnsibleJobLog

## SYNOPSIS
Retrieve job logs.

## SYNTAX

### StdOut (Default)
```
Get-AnsibleJobLog [-Job] <IResource> [-Format <JobLogFormat>] [-Dark] [<CommonParameters>]
```

### Download
```
Get-AnsibleJobLog [-Job] <IResource> [-Download] <DirectoryInfo> [-Format <JobLogFormat>] [-Dark]
 [<CommonParameters>]
```

## DESCRIPTION
Retrieve job logs for UnifiedJob(s) and output to STDOUT or download to files for each jobs.
The job's information is added to the file when downloading.

You can choose log format with `-Format` parameter:  
- `txt` : Plain text (default)  
- `ansi`: Plain text with ANSI color (need Terminal supported VT100 escape sequence)  
- `html`: HTML format  
- `json`: JSON format (only affected when downloding. otherwise same as `ansi`)

"UnifiedJob(s)" referes to following jobs:  
- `Job`             : JobTempalte's job  
- `ProjectUpdate`   : Project's update job  
- `InventoryUpdate` : InventorySource's update job  
- `AdHocCommand`    : AdHocCommand's job  
- `WorkflowJob`     : WorkflowJobTemplate's job  
- `SystemJob`       : SystemJobTemplate's job

When the specified job is a WorkflowJob, retrieve job logs for each of its nodes.

Implements following Rest API:  
- `/api/v2/jobs/{id}/stdout/`  
- `/api/v2/project_updates/{id}/stdout/`  
- `/api/v2/inventory_updates/{id}/stdout/`  
- `/api/v2/ad_hoc_commands/{id}/stdout/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-AnsibleJobLog job:10
==> [10] Job

PLAY [Hello World Sample] ******************************************************

TASK [Gathering Facts] *********************************************************
ok: [localhost]

TASK [Hello Message] ***********************************************************
ok: [localhost] => {
    "msg": "Hello World!"
}

PLAY RECAP *********************************************************************
localhost                  : ok=2    changed=0    unreachable=0    failed=0    skipped=0    rescued=0    ignored=0
```

Show the log for Job of ID 10 as text format.

### Example 2
```powershell
PS C:\> Get-AnsibleJobLog job:10 -Format html -Download .

Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
-a---          2024/08/05    14:20           4081 10.html
```

Download log to the current directory as HTML format.

### Example 3
```powershell
PS C:\> Find-AnsibleJob -Status successful,failed -Count 3 | Get-AnsibleJobLog -Format ansi
==> [13] Job

(snip)

==> [12] Job

(snip)

==> [11] Job

(snip)
```

Retrieve the three jobs finished as successfull or failed, and show the their logs.

## PARAMETERS

### -Dark
Get log with darck(black) background color.
This would only affect HTML format.

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

### -Download
Indicates download to files and download to the argument value's directory.

The file name format: `<job-ID>.<format>`. eg.) 10.txt, 10.ansi, 10.html, 10.json

The only exception is SystemJob, which can only be downloaded as TEXT format.

```yaml
Type: DirectoryInfo
Parameter Sets: Download
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Format
Log format:

- `txt` : Plain text (default)  
- `ansi`: Plain text with ANSI color (need Terminal supported VT100 escape sequence)  
- `html`: HTML format  
- `json`: JSON format (only affected when downloding. otherwise same as `ansi`)  

The only exception is SystemJob, which can only be downloaded as text format.

```yaml
Type: JobLogFormat
Parameter Sets: (All)
Aliases:
Accepted values: txt, ansi, json, html

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Job
UnifiedJob resource object from which to retrieve log.

The resource is an object with `Id` and `Type` properties.
And `Type` should be following value:  
- `Job`             : JobTempalte's job  
- `ProjectUpdate`   : Project's update job  
- `InventoryUpdate` : InventorySource's update job  
- `AdHocCommand`    : AdHocCommand's job  
- `WorkflowJob`     : WorkflowJobTemplate's job  
- `SystemJob`       : SystemJobTemplate's job

> [!TIP]  
> Can specify the resource as string like `Job:1` (Format: `{Type}:{Id}`).
> And also accept objects have `type` and `id` properties.  
>
> For example:  
>  - `-Job (Get-AnsibleJob -Id 1)`  
>  - `-Job @{ type = "job"; id = 1 }`  
>  - `-Job job:1`

```yaml
Type: IResource
Parameter Sets: (All)
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

### Jagabata.IResource
Resource object to which the job log retrieved.
See: `-Job` parameter.

## OUTPUTS

### System.String
Job log string. (when not downloading)

### System.IO.FileInfo
Downloaded file objects.

## NOTES

## RELATED LINKS

[Get-AnsibleJob](Get-AnsibleJob.md)

[Find-AnsibleJob](Find-AnsibleJob.md)

[Get-AnsibleProjectUpdateJob](Get-AnsibleProjectUpdateJob.md)

[Find-AnsibleProjectUpdateJob](Find-AnsibleProjectUpdateJob.md)

[Get-AnsibleInventoryUpdateJob](Get-AnsibleInventoryUpdateJob.md)

[Find-AnsibleInventoryUpdateJob](Find-AnsibleInventoryUpdateJob.md)

[Get-AnsibleSystemJob](Get-AnsibleSystemJob.md)

[Find-AnsibleSystemJob](Find-AnsibleSystemJob.md)

[Get-AnsibleWorkflowJob](Get-AnsibleWorkflowJob.md)

[Find-AnsibleWorkflowJob](Find-AnsibleWorkflowJob.md)

[Get-AnsibleAdHocCommandJob](Get-AnsibleAdHocCommandJob.md)

[Find-AnsibleAdHocCommandJob](Find-AnsibleAdHocCommandJob.md)

[Invoke-AnsibleJobTemplate](Invoke-AnsibleJobTemplate.md)

[Invoke-AnsibleProjectUpdate](Invoke-AnsibleProjectUpdate.md)

[Invoke-AnsibleInventorySource](Invoke-AnsibleInventoryUpdate.md)

[Invoke-AnsibleSystemJobTemplate](Invoke-AnsibleSystemJobTemplate.md)

[Invoke-AnsibleWorkflowJobTemplate](Invoke-AnsibleWorkflowJobTemplate.md)

[Wait-AnsibleUnifiedJob](Wait-AnsibleUnifiedJob.md)
