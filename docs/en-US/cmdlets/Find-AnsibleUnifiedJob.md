---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Find-AnsibleUnifiedJob

## SYNOPSIS
Retrieve Unified Jobs.

## SYNTAX

### All (Default)
```
Find-AnsibleUnifiedJob [-OrderBy <String[]>] [-Search <String[]>] [-Filter <NameValueCollection>]
 [-Count <UInt16>] [-Page <UInt32>] [-All] [<CommonParameters>]
```

### AssociatedWith
```
Find-AnsibleUnifiedJob [-Type] <ResourceType> [-Id] <UInt64> [-OrderBy <String[]>] [-Search <String[]>]
 [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All]
 [<CommonParameters>]
```

### PipelineInput
```
Find-AnsibleUnifiedJob [-Resource] <IResource> [-OrderBy <String[]>] [-Search <String[]>]
 [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All]
 [<CommonParameters>]
```

## DESCRIPTION
Retrieve Jobs which are Job, ProjectUpdate, InventoryUpdate, SystemJob, AdHocCommand or WorkflowJob.

Implementation of following API:  
- `/api/v2/unified_jobs/`  
- `/api/v2/hosts/{id}/ad_hoc_commands/`  
- `/api/v2/groups/{id}/ad_hoc_commands/`  
- `/api/v2/schedules/{id}/jobs/`  
- `/api/v2/instances/{id}/jobs/`  
- `/api/v2/instance_groups/{id}/jobs/`  
- `/api/v2/job_templates/{id}/jobs/`  
- `/api/v2/workflow_job_templates/{id}/workflow_jobs/`  
- `/api/v2/projects/{id}/project_updates/`  
- `/api/v2/inventory_sources/{id}/inventory_updates/`  
- `/api/v2/system_job_templates/{id}/jobs/`  
- `/api/v2/inventories/{id}/ad_hoc_commands/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Find-AnsibleUnifiedJob
```

## PARAMETERS

### -All
Retrieve resources from all pages.

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

### -Count
Number to retrieve per page.

```yaml
Type: UInt16
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: 20
Accept pipeline input: False
Accept wildcard characters: False
```

### -Filter
Filtering various fields.

For examples:  
- `name__icontains=test`: "name" field contains "test" (case-insensitive).  
- `"name_ in=test,demo", created _gt=2024-01-01`: "name" field is "test" or "demo" and created after 2024-01-01.  
- `@{ Name = "name"; Value = "test"; Type = "Contains"; Not = $true }`: "name" field NOT contains "test"

For more details, see [about_Jagabata.psm_Filter_parameter](about_Jagabata.psm_Filter_parameter.md).

```yaml
Type: NameValueCollection
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
Datebase ID of the target resource.
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

### -OrderBy
Retrieve list in the specified orders.
Use `!` prefix to sort in reverse.
Multiple sorting fields are available by separating with a comma(`,`).

Default value: `!id` (descending order of ID)

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: ["!id"]
Accept pipeline input: False
Accept wildcard characters: False
```

### -Page
Page number.

```yaml
Type: UInt32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: 1
Accept pipeline input: False
Accept wildcard characters: False
```

### -Resource
Resource object associated with the resource to be find.

The resource is an object with `Id` and `Type` properties.
And `Type` should be following value:  
- `Group`  
- `Host`  
- `Instance`  
- `InstanceGroup`  
- `Inventory`  
- `InventorySource`  
- `JobTemplate`  
- `Project`  
- `Schedule`  
- `SystemJobTemplate`  
- `WorkflowJobTemplate`

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

### -Search
Search words. (case-insensitive)

Target fields: `name`, `description`, `job__playbook`

Multiple words are available by separating with a comma(`,`).

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

### -Type
Resource type name of the target.
Use in conjection with the `-Id` parameter.

```yaml
Type: ResourceType
Parameter Sets: AssociatedWith
Aliases:
Accepted values: JobTemplate, WorkflowJobTemplate, Project, InventorySource, SystemJobTemplate, Inventory, Host, Group, Schedule, Instance, InstanceGroup

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
- `Group`  
- `Host`  
- `Instance`  
- `InstanceGroup`  
- `Inventory`  
- `InventorySource`  
- `JobTemplate`  
- `Project`  
- `Schedule`  
- `SystemJobTemplate`  
- `WorkflowJobTemplate`

## OUTPUTS

### Jagabata.Resources.IUnifiedJob
Unified Job objects which are following instances implemented `IUnifiedJob`:  
- `Job`             : JobTemplate's job  
- `ProjectUpdate`   : Project Update job  
- `InventoryUpdate` : Inventory Update job  
- `SystemJob`       : SystemJobTemplate's job  
- `AdHocCommand`    : AdHocCommand job  
- `WorkflowJob`     : WorkflowJobTemplate's job

## NOTES

## RELATED LINKS

[Find-AnsibleJob](Find-AnsibleJob.md)

[Find-AnsibleProjectUpdateJob](Find-AnsibleProjectUpdateJob.md)

[Find-AnsibleInventoryUpdateJob](Find-AnsibleInventoryUpdateJob.md)

[Find-AnsibleSystemJob](Find-AnsibleSystemJob.md)

[Find-AnsibleAdHocCommandJob](Find-AnsibleAdHocCommandJob.md)

[Find-AnsibleWorkflowJob](Find-AnsibleWorkflowJob.md)

[Find-AnsibleUnifiedJobTemplate](Find-AnsibleUnifiedJobTemplate.md)
