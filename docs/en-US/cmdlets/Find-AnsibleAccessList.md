---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Find-AnsibleAccessList

## SYNOPSIS
Retrieve Users accessible to a resource.

## SYNTAX

```
Find-AnsibleAccessList [-Resource] <IResource> [-OrderBy <String[]>] [-Search <String[]>]
 [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All] [<CommonParameters>]
```

## DESCRIPTION
Retrieve the list of Users can access to a resource.

Implementation of following API:  
- `/api/v2/instance_groups/{id}/access_list/`  
- `/api/v2/organizations/{id}/access_list/`  
- `/api/v2/users/{id}/access_list/`  
- `/api/v2/projects/{id}/access_list/`  
- `/api/v2/teams/{id}/access_list/`  
- `/api/v2/credentials/{id}/access_list/`  
- `/api/v2/inventories/{id}/access_list/`  
- `/api/v2/job_templates/{id}/access_list/`  
- `/api/v2/workflow_job_templates/{id}/access_list/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-AnsibleOrganization -Id 2 | Find-AnsibleAccessList

Id Type Username     Email           FirstName LastName IsSuperuser IsSystemAuditor Created             Modified            LastLogin           LdapDn ExternalAccount
-- ---- --------     -----           --------- -------- ----------- --------------- -------             --------            ---------           ------ ---------------
 1 User admin        admin@localhost                           True           False 11/04/2023 16:20:25 08/20/2024 17:54:20 08/20/2024 17:54:20
 2 User teramako     *****@*******   tera      mako           False           False 05/21/2024 00:13:43 06/10/2024 22:48:18 06/10/2024 22:48:18
```

Retrieve Users with access to the Organization in ID 2.

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
Resource object to which the search target has access.

The resource is accepted following types:  
- `InstanceGroup`  
- `Organization`  
- `User`  
- `Project`  
- `Team`  
- `Credential`  
- `Inventory`  
- `JobTemplate`  
- `WorkflowJobTemplate`

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
Aliases: associatedWith, r

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Search
Search words. (case-insensitive)

Target fields: `username`, `first_name`, `last_name`, `email`

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

### Jagabata.IResource
Resource object to which the search target has access.

See: `-Resource` parameter.

## OUTPUTS

### Jagabata.Resources.User
## NOTES

## RELATED LINKS

[Find-AnsibleUser](Find-AnsibleUser.md)

[Get-AnsibleUser](Get-AnsibleUser.md)
