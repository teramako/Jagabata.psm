# Jagabata.psm
## about_Jagabata.psm

# SHORT DESCRIPTION
PowerShell module to operate AWX/AnsibleTower using Rest API.

# LONG DESCRIPTION

Can get various information, execute jobs (JobTemplate, Project, InventorySource, AdHocCommand, WorkflowJobTemplate and SystemJobTemplate)
from AWX/AnsibleTower using Rest API.

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

AWX API Reference Guide: https://ansible.readthedocs.io/projects/awx/en/latest/rest_api/api_ref.htm

