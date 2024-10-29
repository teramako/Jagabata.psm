---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Find-AnsibleWorkflowJobTemplate

## SYNOPSIS
Retrieve WorkflowJobTemplates.

## SYNTAX

```
Find-AnsibleWorkflowJobTemplate [[-Organization] <UInt64>] [-OrderBy <String[]>] [-Search <String[]>]
 [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>] [-All] [<CommonParameters>]
```

## DESCRIPTION
Retrieve the list of WorkflowJobTemplates.

Implementation of following API:  
- `/api/v2/workflow_job_templates/`  
- `/api/v2/organizations/{id}/workflow_job_templates/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Find-AnsibleWorkflowJobTemplate
```

### Example 2
```powershell
PS C:\> Find-AnsibleWorkflowJobTemplate -Organization 1
```

Retrieve WorkflowJobTemplates associated with the Organization of ID 1

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

### -Organization
Organization ID or it's object.
Search WorkflowJobTemplates which the Organization is associated with.

```yaml
Type: UInt64
Parameter Sets: (All)
Aliases:

Required: False
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.UInt64
Organization ID or it's object.

## OUTPUTS

### Jagabata.Resources.WorkflowJobTemplate
## NOTES

## RELATED LINKS

[Get-AnsibleWorkflowJobTemplate](Get-AnsibleWorkflowJobTemplate.md)

[Find-AnsibleUnifiedJobTemplate](Find-AnsibleUnifiedJobTemplate.md)

[New-AnsibleWorkflowJobTemplate](New-AnsibleWorkflowJobTemplate.md)

[Update-AnsibleWorkflowJobTemplate](Update-AnsibleWorkflowJobTemplate.md)

[Remove-AnsibleWorkflowJobTemplate](Remove-AnsibleWorkflowJobTemplate.md)

[Get-AnsibleWorkflowJobTemplateNode](Get-AnsibleWorkflowJobTemplateNode.md)

[Find-AnsibleWorkflowJobTemplateNode](Find-AnsibleWorkflowJobTemplateNode.md)

[Invoke-AnsibleWorkflowJobTemplate](Invoke-AnsibleWorkflowJobTemplate.md)

[Start-AnsibleWorkflowJobTemplate](Start-AnsibleWorkflowJobTemplate.md)

[Get-AnsibleWorkflowJob](Get-AnsibleWorkflowJob.md)

[Find-AnsibleWorkflowJob](Find-AnsibleWorkflowJob.md)
