---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Find-AnsibleActivityStream

## SYNOPSIS
Retrieve ActivityStreams.

## SYNTAX

### All (Default)
```
Find-AnsibleActivityStream [-OrderBy <String[]>] [-Search <String[]>] [-Filter <NameValueCollection>]
 [-Count <UInt16>] [-Page <UInt32>] [-All] [<CommonParameters>]
```

### AssociatedWith
```
Find-AnsibleActivityStream [-Type] <ResourceType> [-Id] <UInt64> [-OrderBy <String[]>] [-Search <String[]>]
 [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All] [<CommonParameters>]
```

### PipelineInput
```
Find-AnsibleActivityStream -Resource <IResource> [-OrderBy <String[]>] [-Search <String[]>]
 [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All] [<CommonParameters>]
```

## DESCRIPTION
Retrieve the list of Activity Streams.

Implements following Rest API:

- `/api/v2/activity_stream/`  
- `/api/v2/applications/{id}/activity_stream/`  
- `/api/v2/tokens/{id}/activity_stream/`  
- `/api/v2/organizations/{id}/activity_stream/`  
- `/api/v2/users/{id}/activity_stream/`  
- `/api/v2/projects/{id}/activity_stream/`  
- `/api/v2/teams/{id}/activity_stream/`  
- `/api/v2/credentials/{id}/activity_stream/`  
- `/api/v2/credential_types/{id}/activity_stream/`  
- `/api/v2/inventories/{id}/activity_stream/`  
- `/api/v2/inventory_sources/{id}/activity_stream/`  
- `/api/v2/groups/{id}/activity_stream/`  
- `/api/v2/hosts/{id}/activity_stream/`  
- `/api/v2/job_templates/{id}/activity_stream/`  
- `/api/v2/jobs/{id}/activity_stream/`  
- `/api/v2/ad_hoc_commands/{id}/activity_stream/`  
- `/api/v2/workflow_job_templates/{id}/activity_stream/`  
- `/api/v2/workflow_jobs/{id}/activity_stream/`  
- `/api/v2/execution_environments/{id}/activity_stream/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Find-AnsibleActivityStream -OrderBy timestamp -Count 3
```

Retrieve three ActivityStreams in order by oldest.

### Example 2
```powershell
PS C:\> Find-AnsibleActivityStream -Type Organization -Id 1
```

Retrieve ActivityStreams associated with the Organization of ID 1.

`Id` and `Type` parameters can also be given from the pipeline, likes following:  
    Get-AnsibleOrganization -Id 1 | Find-AnsibleActivityStream

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
Page Number

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
Resource object to which the search target associated with.

The resource is accepted following types:  
- `OAuth2Application`  
- `OAuth2AccessToken`  
- `Organization`  
- `User`  
- `Project`  
- `Team`  
- `Credential`  
- `CredentialType`  
- `Inventory`  
- `InventorySource`  
- `Group`  
- `Host`  
- `JobTemplate`  
- `Job`  
- `AdHocCommand`  
- `WorkflowJobTemplate`  
- `WorkflowJob`  
- `ExecutionEnvironment`

```yaml
Type: IResource
Parameter Sets: PipelineInput
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Search
Search words. (case-insensitive)

Target fields: `changes`

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
Accepted values: OAuth2Application, OAuth2AccessToken, Organization, User, Project, Team, Credential, CredentialType, Inventory, InventorySource, Group, Host, JobTemplate, Job, AdHocCommand, WorkflowJobTemplate, WorkflowJob, ExecutionEnvironment

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
Resource object to which the search target associated with.

See: `-Resource` parameter.

## OUTPUTS

### Jagabata.Resources.ActivityStream

## NOTES

## RELATED LINKS

[Get-AnsibleActivityStream](Get-AnsibleActivityStream.md)
