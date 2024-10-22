# Jagabata.psm
## about_Jagabata.psm

# SHORT DESCRIPTION
PowerShell module to operate AWX/AnsibleTower using Rest API.

# LONG DESCRIPTION

Can get various information, execute jobs (JobTemplate, Project, InventorySource, AdHocCommand, WorkflowJobTemplate or SystemJobTemplate)
from AWX/AnsibleTower using Rest API.

Has a much number of commands (nearly 200). See: Command list [by verb] or [by nouns].

- Retrieve various information (`Get-*`, `Find-*`)
- Launch, wait or stop jobs such as JobTemplate, WorkflowJobTemplate, etc (`Start-*`, `Invoke-*`, `Wait-`, `Stop-*`)
- Creating new resources such as Users, JobTemplates, Credential, etc. (`New-*`)
- Update existing resources (`Update-*`)
- Associate or Unassociate a resource with another resource (`Register-*`, `Unregister-*`)

[by verb]: https://github.com/teramako/Jagabata.psm/blob/develop/docs/en-US/CommandListByVerb.md
[by noun]: https://github.com/teramako/Jagabata.psm/blob/develop/docs/en-US/CommandListByNoun.md

# GET STARTED

1. Install and Import Jagabata.psm (You probably already have this item in place.)
   ```powershell
   Install-Module -Name Jagabata.psm -Scope CurrentUser
   Import-Module Jagabata.psm
   ```
2. Login to AWX/AnsibleTower to obtain a Personal Access Token (PAT) and
   create a configuration file with the [New-ApiConfig] command.  
   See [Settings] for more details.
3. Now you are ready. Now execute your favorite command!

[New-ApiConfig]: https://github.com/teramako/Jagabata.psm/blob/develop/docs/en-US/cmdlets/New-ApiConfig.md
[Settings]: https://github.com/teramako/Jagabata.psm/blob/develop/docs/en-US/settings.md

# EXAMPLES

## 1. Find User

```powershell
PS C:\> Find-User -Search teramako

Id Type Username Email              FirstName LastName IsSuperuser IsSystemAuditor Created            Modified            LastLogin           LdapDn ExternalAccount
-- ---- -------- -----              --------- -------- ----------- --------------- -------            --------            ---------           ------ ---------------
 2 User teramako teramako@gmail.com tera      mako           False           False 2024/05/21 0:13:43 2024/06/10 22:48:18 2024/06/10 22:48:18

```

## 2. Invoke JobTemplate

```powershell
PS C:\> Invoke-JobTemplate -Id 7
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

## 3. Retreive running job and wait for completed

```powershell
PS C:\> Find-Job -Status running -OutVariable jobs

 Id Type Name              JobType LaunchType   Status Finished   Elapsed LaunchedBy     Template  Note
 -- ---- ----              ------- ----------   ------ --------   ------- ----------     --------  ----
121  Job Demo 2                Run     Manual  Running ...            ... ...            ...       ...
120  Job Demo Job Template     Run     Manual  Running ...            ... ...            ...       ...

PS C:\> $jobs | Wait-UnifiedJob
====== [120] Demo Job Template ======

(snip)

====== [121] Demo 2 ======

(snip)


 Id Type Name              JobType LaunchType      Status Finished   Elapsed LaunchedBy     Template  Note
 -- ---- ----              ------- ----------      ------ --------   ------- ----------     --------  ----
121  Job Demo 2                Run     Manual  Successful ...            ... ...            ...       ...
120  Job Demo Job Template     Run     Manual  Successful ...            ... ...            ...       ...

```

# SEE ALSO

AWX API Reference Guide: https://ansible.readthedocs.io/projects/awx/en/latest/rest_api/api_ref.html

