---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Find-Credential

## SYNOPSIS
Retrieve Credentials.

## SYNTAX

### All (Default)
```
Find-Credential [-Kind <String>] [-Galaxy] [-OrderBy <String[]>] [-Search <String[]>]
 [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All] [<CommonParameters>]
```

### AssociatedWith
```
Find-Credential [-Type] <ResourceType> [-Id] <UInt64> [-Kind <String>] [-Galaxy] [-OrderBy <String[]>]
 [-Search <String[]>] [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All]
 [<CommonParameters>]
```

### PipelineInput
```
Find-Credential [-Resource] <IResource> [-Kind <String>] [-Galaxy] [-OrderBy <String[]>] [-Search <String[]>]
 [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All] [<CommonParameters>]
```

## DESCRIPTION
Retrieve the list of Credentials.

Implementation of following API:  
- `/api/v2/credentials/`  
- `/api/v2/organizations/{id}/credentials/`  
- `/api/v2/organizations/{id}/galaxy_credentials/`  
- `/api/v2/users/{id}/credentials/`  
- `/api/v2/teams/{id}/credentials/`  
- `/api/v2/credential_types/{id}/credentials/`  
- `/api/v2/inventory_sources/{id}/credentials/`  
- `/api/v2/inventory_updates/{id}/credentials/`  
- `/api/v2/job_templates/{id}/credentials/`  
- `/api/v2/jobs/{id}/credentials/`  
- `/api/v2/schedules/{id}/credentials/`  
- `/api/v2/workflow_job_template_nodes/{id}/credentials/`  
- `/api/v2/workflow_job_nodes/{id}/credentials/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Find-Credential
```

### Example 2
```powershell
PS C:\> Find-Credential -Type Organization -Id 1
```

Retrieve Credentials associated with the Organization of ID 1.

`Id` and `Type` parameters can also be given from the pipeline, likes following:

    Get-Organization -Id 1 | Find-Credential

### Example 3
```powershell
PS C:\> Find-Credential -Type Organization -Id 1 -Galaxy
```

Retrieve Galaxy Credentials associated with the Organization of ID 1.

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

### -Galaxy
Only affected for an Organization type.
Retrieve Galaxy Credentials instead of normal Credentials.

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

### -Kind
Filter with `kind` field.

Examples.)
`ssh`, `scm`, `vault`, `net`, `awx`, `openstack`, `vmware`, `satellite6`, `gce`, `azure_rm`,
`github_token`, `gitlab_token`, `insights`, `rhv`, `controller`, `kubernetes_bearer_token`,
`registry`, `galaxy_api_token`, `gpg_public_key`, `aim`, `aws_secretsmanager_credential`,
`azure_kv`, `centrify_vault_kv`, `conjur`, `hashivault_kv`, `hashivault_ssh`, `thycotic_dsv`,
`thycotic_tss`

> [!NOTE]  
> The `kind` field of a Credential corresponds to `namespace` field of a CredentialType.

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

### -OrderBy
Retrieve list in the specified orders.
Use `!` prefix to sort in reverse.
Multiple sorting fields are available by separating with a comma(`,`).

Default value: `id` (ascending order of ID)

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: ["id"]
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
Resource object to which the search target associated with.

The resource is accepted following types:  
- `Organization`  
- `User`  
- `Team`  
- `CredentialType`  
- `InventorySource`  
- `InventoryUpdate`  
- `JobTemplate`  
- `Job`  
- `Schedule`  
- `WorkflowJobTemplateNode`  
- `WorkflowJobNode`

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

Target fields: `name`, `description`

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
Accepted values: Organization, User, Team, CredentialType, InventorySource, InventoryUpdate, JobTemplate, Job, Schedule, WorkflowJobTemplateNode, WorkflowJobNode

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

### Jagabata.Resources.Credential
## NOTES

## RELATED LINKS

[Get-Credential](Get-Credential.md)

[Get-CredentialType](Get-CredentialType.md)

[Find-CredentialType](Find-CredentialType.md)

[New-Credential](New-Credential.md)

[Update-Credential](Update-Credential.md)

[Register-Credential](Register-Credential.md)

[Unregister-Credential](Unregister-Credential.md)

[Remove-Credential](Remove-Credential.md)
