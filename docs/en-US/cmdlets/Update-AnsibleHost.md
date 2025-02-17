---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Update-AnsibleHost

## SYNOPSIS
Update a Host.

## SYNTAX

```
Update-AnsibleHost [-Id] <UInt64> [-Name <String>] [-Description <String>] [-Enabled <Boolean>]
 [-InstanceId <String>] [-Variables <String>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Update a Host.

Implements following Rest API:  
- `/api/v2/hosts/{id}/` (PATCH)

## EXAMPLES

### Example 1
```powershell
PS C:\> Update-AnsibleHost -Id 3 -Enabled $true -Variables @{ ansible_host = "192.168.0.100" }
```

Update variable and activate for the Host of ID 3.

## PARAMETERS

### -Description
Optional description of the host.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Enabled
Is the host online and available for running jobs?

```yaml
Type: Boolean
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
Host ID or its resource object to be updated.

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

### -InstanceId
Used by the remote inventory source to uniquely identify the host.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
Name of the host.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Variables
Specify in JSON or YAML format.
You can also specify an object of type `IDictionary` as a parameter value.

Example: `-Variables @{ ansible_host = "192.168.0.10"; ansible_connection = "ssh"; }`

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
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
Host ID or its resource object to be updated.
See `-Id` parameter.

## OUTPUTS

### Jagabata.Resources.Host
Updated Host object.

## NOTES

## RELATED LINKS

[Get-AnsibleHost](Get-AnsibleHost.md)

[Find-AnsibleHost](Find-AnsibleHost.md)

[New-AnsibleHost](New-AnsibleHost.md)

[Register-AnsibleHost](Register-AnsibleHost.md)

[Unregister-AnsibleHost](Unregister-AnsibleHost.md)

[Remove-AnsibleHost](Remove-AnsibleHost.md)
