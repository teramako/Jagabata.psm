---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Disable-NotificationTemplate

## SYNOPSIS
Disable a NotificationTemplate.

## SYNTAX

```
Disable-NotificationTemplate [-Id] <UInt64> [-For] <IResource> [-On] <String[]> [-WhatIf] [-Confirm]
 [<CommonParameters>]
```

## DESCRIPTION
Disable notification at the specified timings (`Started`, `Success`, `Error` and `Approval`)
for the target resource (`Organization`, `Project`, `InventorySource`, `JobTemplate`, `SystemJobTemplate` and `WorkflowJobTemplate`).
The timing `Approval` is only available for `Organization` and `WorkflowJobTemplate`.

Implements following Rest API:  
- `/api/v2/organizations/{id}/notification_templates_approvals/` (POST)  
- `/api/v2/organizations/{id}/notification_templates_error/` (POST)  
- `/api/v2/organizations/{id}/notification_templates_started/` (POST)  
- `/api/v2/organizations/{id}/notification_templates_success/` (POST)  
- `/api/v2/projects/{id}/notification_templates_error/` (POST)  
- `/api/v2/projects/{id}/notification_templates_started/` (POST)  
- `/api/v2/projects/{id}/notification_templates_success/` (POST)  
- `/api/v2/inventory_sources/{id}/notification_templates_error/` (POST)  
- `/api/v2/inventory_sources/{id}/notification_templates_started/` (POST)  
- `/api/v2/inventory_sources/{id}/notification_templates_success/` (POST)  
- `/api/v2/job_templates/{id}/notification_templates_error/` (POST)  
- `/api/v2/job_templates/{id}/notification_templates_started/` (POST)  
- `/api/v2/job_templates/{id}/notification_templates_success/` (POST)  
- `/api/v2/system_job_templates/{id}/notification_templates_error/` (POST)  
- `/api/v2/system_job_templates/{id}/notification_templates_started/` (POST)  
- `/api/v2/system_job_templates/{id}/notification_templates_success/` (POST)  
- `/api/v2/workflow_job_templates/{id}/notification_templates_approvals/` (POST)  
- `/api/v2/workflow_job_templates/{id}/notification_templates_error/` (POST)  
- `/api/v2/workflow_job_templates/{id}/notification_templates_started/` (POST)  
- `/api/v2/workflow_job_templates/{id}/notification_templates_success/` (POST)

## EXAMPLES

### Example 1
```powershell
PS C:\> Disable-NotificationTemplate -Id 5 -For (Get-JobTemplate -Id 3) -On Success,Error
```

## PARAMETERS

### -For
The target resource to be enabled the notification.

The available resources:  
- `Organization`  
- `Project`  
- `InventorySource`  
- `JobTemplate`  
- `SystemJobTemplate`  
- `WorkflowJobTemplate`

```yaml
Type: IResource
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
NotificationTemplate ID.

```yaml
Type: UInt64
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -On
Timing of notifications to be enabled.

Available timings:  
- `Started`  
- `Success`  
- `Error`  
- `Approval` (only for `Organization` and `WorkflowJobTemplate` resource)

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:
Accepted values: Started, Success, Error, Approval

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Confirm
Prompts you for confirmation before running the cmdlet.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: cf

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -WhatIf
Shows what would happen if the cmdlet runs.
The cmdlet is not run.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: wi

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
NotificationTemplate ID.

## OUTPUTS

### None
## NOTES

## RELATED LINKS

[Get-NotificationTemplate](Get-NotificationTemplate.md)

[Find-NotificationTemplate](Find-NotificationTemplate.md)

[New-NotificationTemplate](New-NotificationTemplate.md)

[Enable-NotificationTemplate](Enable-NotificationTemplate.md)

[Update-NotificationTemplate](Update-NotificationTemplate.md)

[Remove-NotificationTemplate](Remove-NotificationTemplate.md)
