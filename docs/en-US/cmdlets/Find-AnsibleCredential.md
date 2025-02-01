---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Find-AnsibleCredential

## SYNOPSIS
Retrieve Credentials.

## SYNTAX

```
Find-AnsibleCredential [[-Resource] <IResource>] [-CredentialTypeKind <CredentialTypeKind[]>]
 [-CredentialTypeNamespace <String[]>] [-Galaxy] [-OrderBy <String[]>] [-Search <String[]>]
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
PS C:\> Find-AnsibleCredential
```

### Example 2
```powershell
PS C:\> Find-AnsibleCredential -Resource Organization:1
```

Retrieve Credentials associated with the Organization of ID 1.

`Resource` parameter can also be given from the pipeline, likes following:

    Get-AnsibleOrganization -Id 1 | Find-AnsibleCredential

### Example 3
```powershell
PS C:\> Find-AnsibleCredential -Resource Organization:1 -Galaxy
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

### -CredentialTypeKind
Filter with CredentialType's `kind` field.
This parameter is the same as `-Filter credential_type__kind__in=...`.

```yaml
Type: CredentialTypeKind[]
Parameter Sets: (All)
Aliases: Kind
Accepted values: ssh, vault, net, scm, cloud, registry, token, insights, external, kubernetes, galaxy, cryptography

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -CredentialTypeNamespace
Filter with `kind` field.
This parameter is the same as `-Filter credential_type__namespace__in=...`.

Examples.)
`ssh`, `scm`, `vault`, `net`, `awx`, `openstack`, `vmware`, `satellite6`, `gce`, `azure_rm`,
`github_token`, `gitlab_token`, `insights`, `rhv`, `controller`, `kubernetes_bearer_token`,
`registry`, `galaxy_api_token`, `gpg_public_key`, `aim`, `aws_secretsmanager_credential`,
`azure_kv`, `centrify_vault_kv`, `conjur`, `hashivault_kv`, `hashivault_ssh`, `thycotic_dsv`,
`thycotic_tss`

> [!NOTE]  
> The `kind` field of a Credential corresponds to `namespace` field of a CredentialType.

```yaml
Type: String[]
Parameter Sets: (All)
Aliases: Namespace

Required: False
Position: Named
Default value: None
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

> [!TIP]  
> Can specify the resource as string like `User:1` (Format: `{Type}:{Id}`).
> And also accept objects have `type` and `id` properties.  
>
> For example:  
>  - `-Resource (Get-AnsibleUser -Id 1)`  
>  - `-Resource @{ type = "user"; id = 1 }`  
>  - `-Resource user:1`

```yaml
Type: IResource
Parameter Sets: (All)
Aliases:

Required: False
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

[Get-AnsibleCredential](Get-AnsibleCredential.md)

[Get-AnsibleCredentialType](Get-AnsibleCredentialType.md)

[Find-AnsibleCredentialType](Find-AnsibleCredentialType.md)

[New-AnsibleCredential](New-AnsibleCredential.md)

[Update-AnsibleCredential](Update-AnsibleCredential.md)

[Register-AnsibleCredential](Register-AnsibleCredential.md)

[Unregister-AnsibleCredential](Unregister-AnsibleCredential.md)

[Remove-AnsibleCredential](Remove-AnsibleCredential.md)
