---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Find-AnsibleWorkflowApprovalRequest

## SYNOPSIS
Retrieve request jobs for WorkflowApproval.

## SYNTAX

```
Find-AnsibleWorkflowApprovalRequest [[-WorkflowApprovalTemplate] <UInt64>] [-Status <JobStatus[]>]
 [-OrderBy <String[]>] [-Search <String[]>] [-Filter <NameValueCollection>] [-Count <UInt16>] [-Page <UInt32>]
 [-All] [<CommonParameters>]
```

## DESCRIPTION
Retrieve the list of WorkflowApproval request jobs.

Implementation of following API:  
- `/api/v2/workflow_approvals/`  
- `/api/v2/workflow_approval_templates/{id}/approvals/`

## EXAMPLES

### Example 1
```powershell
PS C:\> Find-AnsibleWorkflowApprovalRequest -Status pending
```

Retrieve WorkflowApprovals in the pending status.

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

### -Status
Filter by status.

```yaml
Type: JobStatus[]
Parameter Sets: (All)
Aliases:
Accepted values: Pending, Successful, Failed

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -WorkflowApprovalTemplate
WorkflowApprovalTemplate ID or it's object.

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable, -ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.UInt64
WorkflowApprovalTemplate ID or it's object.

## OUTPUTS

### Jagabata.Resources.WorkflowApproval
## NOTES

## RELATED LINKS

[Get-AnsibleWorkflowApprovalRequest](Get-AnsibleWorkflowApprovalRequest.md)

[Approve-AnsibleWorkflowApprovalRequest](Approve-AnsibleWorkflowApprovalRequest.md)

[Deny-AnsibleWorkflowApprovalRequest](Deny-AnsibleWorkflowApprovalRequest.md)

[Remove-AnsibleWorkflowApprovalRequest](Remove-AnsibleWorkflowApprovalRequest.md)
