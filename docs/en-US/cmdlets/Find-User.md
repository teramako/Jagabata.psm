---
external help file: AWX.psm.dll-Help.xml
Module Name: AWX.psm
online version:
schema: 2.0.0
---

# Find-User

## SYNOPSIS
Retrieve Users.

## SYNTAX

### All (Default)
```
Find-User [[-UserName] <String[]>] [[-Email] <String[]>] [-OrderBy <String[]>] [-Search <String[]>]
 [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All] [<CommonParameters>]
```

### AssociatedWith
```
Find-User [-Type] <ResourceType> [-Id] <UInt64> [[-UserName] <String[]>] [[-Email] <String[]>]
 [-OrderBy <String[]>] [-Search <String[]>] [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>]
 [-All] [<CommonParameters>]
```

### PipelineInput
```
Find-User [-Resource] <IResource> [[-UserName] <String[]>] [[-Email] <String[]>] [-OrderBy <String[]>]
 [-Search <String[]>] [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All]
 [<CommonParameters>]
```

## DESCRIPTION
Retrieve the list of Users.

Implementation of following API:  
- `/api/v2/users/`  
- `/api/v2/organizations/{id}/users/`  
- `/api/v2/teams/{id}/users/`  
- `/api/v2/credentials/{id}/users/`  
- `/api/v2/roles/{id}/users/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Find-User
```

### Example 2
```powershell
PS C:\> Find-User -Type Organization -Id 1
```

Retrieve Users associated with the Organization of ID 1

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

### -Email
Filter by `email` field.

Multiple addresses are available by separating with a comma(`,`).

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: 3
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

For more details, see [about_AWX.psm_Filter_parameter](about_AWX.psm_Filter_parameter.md).

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
- `Team`  
- `Credential`  
- `Role`

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

### -Type
Resource type name of the target.
Use in conjection with the `-Id` parameter.

```yaml
Type: ResourceType
Parameter Sets: AssociatedWith
Aliases:
Accepted values: Organization, Team, Credential, Role

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -UserName
Filter by `user_name` field.

Multiple names are available by separating with a comma(`,`).

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### AWX.Resources.IResource
Resource object to which the search target associated with. 

See: `-Resource` parameter.

## OUTPUTS

### AWX.Resources.User
## NOTES

## RELATED LINKS

[Get-User](Get-User.md)

[New-User](New-User.md)

[Update-User](Update-User.md)

[Register-User](Register-User.md)

[Unregister-User](Unregister-User.md)

[Remove-User](Remove-User.md)

[Get-Team](Get-Team.md)

[Find-Team](Find-Team.md)
