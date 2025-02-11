---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Find-AnsibleHost

## SYNOPSIS
Retrieve Hosts.

## SYNTAX

```
Find-AnsibleHost [[-Resource] <IResource>] [-OnlyChildren] [-OrderBy <String[]>] [-Search <String[]>]
 [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All] [<CommonParameters>]
```

## DESCRIPTION
Retrieve the list of Hosts.

Implementation of following API:  
- `/api/v2/hosts/`  
- `/api/v2/inventories/{id}/hosts/`  
- `/api/v2/inventory_sources/{id}/hosts/`  
- `/api/v2/groups/{id}/hosts/`  
- `/api/v2/groups/{id}/all_hosts/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Find-AnsibleHost
```

### Example 2
```powershell
PS C:\> Find-AnsibleHost -Resource Inventory:1
```

Retrieve Hosts associated with the Inventory of ID 1

`Resource` parameter can also be given from the pipeline, likes following:  
    Get-AnsibleInventory -Id 1 | Find-AnsibleHost

### Example 3
```powershell
PS C:\> Find-AnsibleHost -Resource Group:1
```

Retrieve Hosts directly or indirectly belonging to the target Group (ID 1).

If you need only directly members, use `-OnlyChildren` parameter.

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

### -OnlyChildren
List only directly member group.
Only affected for a Group Type

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
- `Inventory`  
- `InventorySource`  
- `Group`

> [!TIP]  
> Can specify the resource as string like `Group:1` (Format: `{Type}:{Id}`).
> And also accept objects have `type` and `id` properties.  
>
> For example:  
>  - `-Resource (Get-AnsibleGroup -Id 1)`  
>  - `-Resource @{ type = "group"; id = 1 }`  
>  - `-Resource group:1`

```yaml
Type: IResource
Parameter Sets: (All)
Aliases: associatedWith, r

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

### Jagabata.IResource
Resource object to which the search target associated with.
See: `-Resource` parameter.

## OUTPUTS

### Jagabata.Resources.Host
## NOTES

## RELATED LINKS

[Get-AnsibleHost](Get-AnsibleHost.md)

[Get-AnsibleInventory](Get-AnsibleInventory.md)

[Find-AnsibleInventory](Find-AnsibleInventory.md)

[Get-AnsibleGroup](Get-AnsibleGroup.md)

[Find-AnsibleGroup](Find-AnsibleGroup.md)

[New-AnsibleHost](New-AnsibleHost.md)

[Update-AnsibleHost](Update-AnsibleHost.md)

[Register-AnsibleHost](Register-AnsibleHost.md)

[Unregister-AnsibleHost](Unregister-AnsibleHost.md)

[Remove-AnsibleHost](Remove-AnsibleHost.md)
