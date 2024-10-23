---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Find-AnsibleOrganization

## SYNOPSIS
Retrieve Organizations.

## SYNTAX

### All (Default)
```
Find-AnsibleOrganization [[-Name] <String[]>] [-OrderBy <String[]>] [-Search <String[]>]
 [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All]
 [<CommonParameters>]
```

### User
```
Find-AnsibleOrganization -User <UInt64> [-Admin] [[-Name] <String[]>] [-OrderBy <String[]>]
 [-Search <String[]>] [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All]
 [<CommonParameters>]
```

## DESCRIPTION
Retrieve the list of Organizations.

Implementation of following API:  
- `/api/v2/organizations/`  
- `/api/v2/users/{id}/organizations/`  
- `/api/v2/users/{id}/admin_of_organizations/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Find-AnsibleOrganization
```

### Example 2
```powershell
PS C:\> Find-AnsibleOrganization -User 1
```

Retrieve Organizations which the User of ID 1 belong to.

### Example 3
```powershell
PS C:\> Find-AnsibleJobTemplate -User 1 -Admin
```

Retrieve Organizations administrered by the User of ID 1.

## PARAMETERS

### -Admin
Filter to Organizations administerted by the target User.

```yaml
Type: SwitchParameter
Parameter Sets: User
Aliases:

Required: False
Position: Named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

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

### -Name
Filter by Organization name.
The names must be an exact match. (case-sensitive)

Multiple words are available by separating with a comma(`,`).

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: 0
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

### -User
User ID or it's object.
Retrieve Organizations which the User belong to.

> [!TIP]  
> Can specify `IResource` object.  
> For example: `-Id (Get-AnsibleUser -Id 3)`, `-Id $users[1]`

```yaml
Type: UInt64
Parameter Sets: User
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.UInt64
User ID or it's object.
See: `-User` parameter.

## OUTPUTS

### Jagabata.Resources.Organization
## NOTES

## RELATED LINKS

[Get-AnsibleOrganization](Get-AnsibleOrganization.md)

[New-AnsibleOrganization](New-AnsibleOrganization.md)

[Update-AnsibleOrganization](Update-AnsibleOrganization.md)

[Remove-AnsibleOrganization](Remove-AnsibleOrganization.md)
