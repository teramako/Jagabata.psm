---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Update-AnsibleExecutionEnvironment

## SYNOPSIS
Update an ExecutionEnvironment.

## SYNTAX

```
Update-AnsibleExecutionEnvironment [-Id] <UInt64> [-Name <String>] [-Description <String>] [-Image <String>]
 [-Organization <UInt64>] [-Credential <UInt64>] [-Pull <String>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Update an ExecutionEnvironment.

Implements following Rest API:  
- `/api/v2/execution_environments/{id}/` (PATCH)

## EXAMPLES

### Example 1
```powershell
PS C:\> Update-AnsibleExecutionEnvironment -Id 4 -Pull missing
```

## PARAMETERS

### -Credential
Credential ID for container registry (kind = `registry`).
Specify a `$null` or `0` value to clear the set value.

```yaml
Type: UInt64
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Description
Optional description of the ExecutionEnvironment.

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

### -Id
ExecutionEnvironment ID or its resource object to be updated.

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

### -Image
The full image location, including the container registry, image name, and version tag.

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
Name of the ExecutionEnvironment.

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

### -Organization
Organization ID or its resource object used to determine access to the ExecutionEnvironment.

```yaml
Type: UInt64
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Pull
Pull image before running:  
- ``: Unspecified.
- `always`: Always pull container before running.  
- `missing`: Only pull the image if not present before running.  
- `never`: Never pull container before running.

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: , always, missing, never

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
ExecutionEnvironment ID or its resource object to be updated.
See `-Id` parameter.

## OUTPUTS

### Jagabata.Resources.ExecutionEnvironment
Update ExecutionEnvironment object.

## NOTES

## RELATED LINKS

[Get-AnsibleExecutionEnvironment](Get-AnsibleExecutionEnvironment.md)

[Find-AnsibleExecutionEnvironment](Find-AnsibleExecutionEnvironment.md)

[New-AnsibleExecutionEnvironment](New-AnsibleExecutionEnvironment.md)

[Remove-AnsibleExecutionEnvironment](Remove-AnsibleExecutionEnvironment.md)
