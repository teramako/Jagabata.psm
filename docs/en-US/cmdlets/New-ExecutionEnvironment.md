---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# New-ExecutionEnvironment

## SYNOPSIS
Create an ExecutionEnvironment.

## SYNTAX

```
New-ExecutionEnvironment -Name <String> [-Description <String>] -Image <String> [-Organization <UInt64>]
 [-Credential <UInt64>] [-Pull <String>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Credential an ExecutionEnvironment.

Implements following Rest API:  
- `/api/v2/execution_environments/` (POST)

## EXAMPLES

### Example 1
```powershell
PS C:\> New-ExecutionEnvironment -Name "AWX-EE 2" -Image quary.io/ansible/awx-ee:latest
```

## PARAMETERS

### -Credential
Credential ID for container registry (kind = `registry`).

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

### -Image
The full image location, including the container registry, image name, and version tag.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
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

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Organization
Organization ID used to determine access to the ExecutionEnvironment.

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
- ``: (defualt)  
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

### None
## OUTPUTS

### Jagabata.Resources.ExecutionEnvironment
New created ExecutionEnvironment object.

## NOTES

## RELATED LINKS

[Get-ExecutionEnvironment](Get-ExecutionEnvironment.md)

[Find-ExecutionEnvironment](Find-ExecutionEnvironment.md)

[Update-ExecutionEnvironment](Update-ExecutionEnvironment.md)

[Remove-ExecutionEnvironment](Remove-ExecutionEnvironment.md)
