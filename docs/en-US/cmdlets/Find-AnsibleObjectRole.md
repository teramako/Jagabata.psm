---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Find-AnsibleObjectRole

## SYNOPSIS
Retrieve Roles for the target resource.

## SYNTAX

```
Find-AnsibleObjectRole [-Resource] <IResource> [-OrderBy <String[]>] [-Search <String[]>]
 [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All] [<CommonParameters>]
```

## DESCRIPTION
Retrieve the list of Roles for the target resource.

Implementation of following API:  
- `/api/v2/instance_groups/{id}/object_roles/`  
- `/api/v2/organizations/{id}/object_roles/`  
- `/api/v2/projects/{id}/object_roles/`  
- `/api/v2/teams/{id}/object_roles/`  
- `/api/v2/credentials/{id}/object_roles/`  
- `/api/v2/inventories/{id}/object_roles/`  
- `/api/v2/job_templates/{id}/object_roles/`  
- `/api/v2/workflow_job_templates/{id}/object_roles/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Find-AnsibleObjectRole -Resource Organization:1
```

Retrieve Roles for the Organization of ID 1.

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
Resource object to which the search target associated with.

The resource is accepted following types:  
- `InstanceGroup`  
- `Organization`  
- `Project`  
- `Team`  
- `Credential`  
- `Inventory`  
- `JobTemplate`  
- `WorkflowJobTemplate`

> [!TIP]  
> Can specify the resource as string like `Organization:1` (Format: `{Type}:{Id}`).
> And also accept objects have `type` and `id` properties.  
>
> For example:  
>  - `-Resource (Get-AnsibleOrganization -Id 1)`  
>  - `-Resource @{ type = "organization"; id = 1 }`  
>  - `-Resource organization:1`

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

Target fields: `role_field`, `content_type__model`

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
Resource object to which the search target associated with.
See: `-Resource` parameter.

## OUTPUTS

### Jagabata.Resources.Role
## NOTES

## RELATED LINKS

[Get-AnsibleRole](Get-AnsibleRole.md)

[Find-AnsibleRole](Find-AnsibleRole.md)

[Grant-AnsibleRole](Grant-AnsibleRole.md)

[Revoke-AnsibleRole](Revoke-AnsibleRole.md)
