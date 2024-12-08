---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Find-AnsibleGroup

## SYNOPSIS
Retrieve Groups.

## SYNTAX

### All (Default)
```
Find-AnsibleGroup [-OrderBy <String[]>] [-Search <String[]>] [-Filter <NameValueCollection>] [-Count <UInt16>]
 [-Page <UInt32>] [-All] [<CommonParameters>]
```

### AssociatedWith
```
Find-AnsibleGroup [-Type] <ResourceType> [-Id] <UInt64> [-OnlyRoot] [-OnlyParnets] [-OrderBy <String[]>]
 [-Search <String[]>] [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All]
 [<CommonParameters>]
```

### PipelineInput
```
Find-AnsibleGroup -Resource <IResource> [-OnlyRoot] [-OnlyParnets] [-OrderBy <String[]>] [-Search <String[]>]
 [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All] [<CommonParameters>]
```

## DESCRIPTION
Retrieve the list of Groups.

Implementation of following API:  
- `/api/v2/groups/`  
- `/api/v2/inventories/{id}/groups/`  
- `/api/v2/inventories/{id}/root_groups/`  
- `/api/v2/groups/{id}/children/`  
- `/api/v2/inventory_sources/{id}/groups/`  
- `/api/v2/hosts/{id}/groups/`  
- `/api/v2/hosts/{id}/all_groups/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Find-AnsibleGroup
```

### Example 2
```powershell
PS C:\> Find-AnsibleGroup -Type Inventory -Id 1
```

Retrieve Groups associated with the Inventory of ID 1

`Id` and `Type` parameters can also be given from the pipeline, likes following:  
    Get-AnsibleInventory -Id 1 | Find-AnsibleGroup

### Example 3
```powershell
PS C:\> Find-AnsibleGroup -Type Inventory -Id 1 -OnlyRoot
```

Retrieve **root** (top-level) Groups associated with the Inventory of ID 1

### Example 4
```powershell
PS C:\> Find-AnsibleGroup -Type Host -Id 1
```

Retrieve Groups of which the target Host (ID 1) is directly or indirectly a member.

If you need only directly a member, use `-OnlyParents` parameter.

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

### -OnlyParnets
List only directly member Groups.
Only affected for a **Host** Type

```yaml
Type: SwitchParameter
Parameter Sets: AssociatedWith, PipelineInput
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -OnlyRoot
List only root(Top-level) Groups.
Only affected for an **Inventory** Type

```yaml
Type: SwitchParameter
Parameter Sets: AssociatedWith, PipelineInput
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
- `Group`  
- `InventorySource`  
- `Host`

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
Accepted values: Inventory, Group, InventorySource, Host

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

### Jagabata.Resources.Group
## NOTES

## RELATED LINKS

[Get-AnsibleGroup](Get-AnsibleGroup.md)

[Get-AnsibleInventory](Get-AnsibleInventory.md)

[Find-AnsibleInventory](Find-AnsibleInventory.md)

[Get-AnsibleHost](Get-AnsibleHost.md)

[Find-AnsibleHost](Find-AnsibleHost.md)

[New-AnsibleGroup](New-AnsibleGroup.md)

[Update-AnsibleGroup](Update-AnsibleGroup.md)

[Register-AnsibleGroup](Register-AnsibleGroup.md)

[Unregister-AnsibleGroup](Unregister-AnsibleGroup.md)

[Remove-AnsibleGroup](Remove-AnsibleGroup.md)
