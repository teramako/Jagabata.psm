---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# Update-AnsibleProject

## SYNOPSIS
Update a Project.

## SYNTAX

```
Update-AnsibleProject [-Id] <UInt64> [-Name <String>] [-Description <String>] [-Organization <UInt64>]
 [-ScmType <String>] [-DefaultEnvironment <UInt64>] [-SignatureValidationCredential <UInt64>]
 [-LocalPath <String>] [-ScmUrl <String>] [-ScmBranch <String>] [-ScmRefspec <String>] [-Credential <UInt64>]
 [-ScmClean <Boolean>] [-ScmDeleteOnUpdate <Boolean>] [-ScmTrackSubmodules <Boolean>]
 [-ScmUpdateOnLaunch <Boolean>] [-AllowOverride <Boolean>] [-Timeout <Int32>] [-WhatIf] [-Confirm]
 [<CommonParameters>]
```

## DESCRIPTION
Update a Project. 

Implements following Rest API:  
- `/api/v2/projects/{id}/` (PATCH)

## EXAMPLES

### Example 1
```powershell
PS C:\> Update-AnsibleProject -Id 10 -ScmBranch develop -AllowOverride $true
```

## PARAMETERS

### -AllowOverride
Allow changing the SCM branch or revision in a job template that uses the project.

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

### -Credential
Credential ID.
Must be `scm` kind credential.

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

### -DefaultEnvironment
ExecutionEnvirontment ID or its resource object to be as default Execution Environment for jobs run using the project.

> [!TIP]  
> Specify `0` or `$null` if want to set empty.

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
Optional description of the project.

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
Project ID or its resource object to be updated.

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

### -LocalPath
Local path (relative to `PROJECT_ROOT`) containing playbooks and related files for the project.

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
Name of the project.

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
Organization ID or its resource object.

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

### -ScmBranch
Specific branch, tag or commit to checkout.

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

### -ScmClean
Discard any local changes before syncing the project.

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

### -ScmDeleteOnUpdate
Delete the project before syncing.

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

### -ScmRefspec
Fit git projects, an additional refspec to fetch.

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

### -ScmTrackSubmodules
Track submodules latest commits on defined branch.

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

### -ScmType
Specifies the source control system used to store the project.

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: , Manual, Git, Subversion, Insights, RemoteArchive

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ScmUpdateOnLaunch
Update the project when a job is launched that uses the project.

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

### -ScmUrl
The location where the project is stored.

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

### -SignatureValidationCredential
Credential ID or its resource object.
This is an optional credential used for validating files in the project against unexpected changes.

> [!TIP]  
> Specify `0` or `$null` if want to set empty.

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

### -Timeout
The amount of time (in seconds) to run before the task is canceled.

```yaml
Type: Int32
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
Project ID or its resource object to be updated.
See `-Id` parameter.

## OUTPUTS

### Jagabata.Resources.Project
Updated Project object.

## NOTES

## RELATED LINKS

[Get-AnsibleProject](Get-AnsibleProject.md)

[Find-AnsibleProject](Find-AnsibleProject.md)

[New-AnsibleProject](New-AnsibleProject.md)

[Remove-AnsibleProject](Remove-AnsibleProject.md)
