---
external help file: Jagabata.psm.dll-Help.xml
Module Name: Jagabata.psm
online version:
schema: 2.0.0
---

# New-AnsibleProject

## SYNOPSIS
Create a Project.

## SYNTAX

### Manual (Default)
```
New-AnsibleProject [-Local] -Name <String> [-Description <String>] -Organization <UInt64>
 [-DefaultEnvironment <UInt64>] [-SignatureValidationCredential <UInt64>] -LocalPath <String>
 [-Timeout <Int32>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

### Git
```
New-AnsibleProject [-Git] -Name <String> [-Description <String>] -Organization <UInt64>
 [-DefaultEnvironment <UInt64>] [-SignatureValidationCredential <UInt64>] -ScmUrl <String>
 [-ScmBranch <String>] [-ScmRefspec <String>] [-Credential <UInt64>] [-ScmClean] [-ScmDeleteOnUpdate]
 [-ScmTrackSubmodules] [-ScmUpdateOnLaunch] [-AllowOverride] [-Timeout <Int32>] [-WhatIf] [-Confirm]
 [<CommonParameters>]
```

### Svn
```
New-AnsibleProject [-Subversion] -Name <String> [-Description <String>] -Organization <UInt64>
 [-DefaultEnvironment <UInt64>] [-SignatureValidationCredential <UInt64>] -ScmUrl <String>
 [-ScmBranch <String>] [-Credential <UInt64>] [-ScmClean] [-ScmDeleteOnUpdate] [-ScmUpdateOnLaunch]
 [-AllowOverride] [-Timeout <Int32>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

### Insights
```
New-AnsibleProject [-Insights] -Name <String> [-Description <String>] -Organization <UInt64>
 [-DefaultEnvironment <UInt64>] [-SignatureValidationCredential <UInt64>] -Credential <UInt64> [-ScmClean]
 [-ScmDeleteOnUpdate] [-ScmUpdateOnLaunch] [-Timeout <Int32>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

### Archive
```
New-AnsibleProject [-RemoteArchive] -Name <String> [-Description <String>] -Organization <UInt64>
 [-DefaultEnvironment <UInt64>] [-SignatureValidationCredential <UInt64>] -ScmUrl <String>
 [-Credential <UInt64>] [-ScmClean] [-ScmDeleteOnUpdate] [-ScmUpdateOnLaunch] [-AllowOverride]
 [-Timeout <Int32>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Create a Project.

Implements following Rest API:  
- `/api/v2/projects/` (POST)

## EXAMPLES

### Example 1
```powershell
PS C:\> New-AnsibleProject -Git -Name GitProject2 -Organization 2 -ScmUrl git@githost:repo1.git -Credential 2
```

## PARAMETERS

### -AllowOverride
Allow changing the SCM branch or revision in a job template that uses the project.

```yaml
Type: SwitchParameter
Parameter Sets: Git, Svn, Archive
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
Parameter Sets: Git, Svn, Archive
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

```yaml
Type: UInt64
Parameter Sets: Insights
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DefaultEnvironment
The default Execution Environment for jobs run using the project.

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

### -Git
Create a "git" type project.

```yaml
Type: SwitchParameter
Parameter Sets: Git
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Insights
Create a "insights" (Red Hat Insights) type project.

```yaml
Type: SwitchParameter
Parameter Sets: Insights
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Local
Create a Manual type project.

```yaml
Type: SwitchParameter
Parameter Sets: Manual
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -LocalPath
Local path (relative to `PROJECT_ROOT`) containing playbooks and related files for the project.

```yaml
Type: String
Parameter Sets: Manual
Aliases:

Required: True
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

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Organization
Organization ID.

```yaml
Type: UInt64
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -RemoteArchive
Create a "Remote Archive" type project.

```yaml
Type: SwitchParameter
Parameter Sets: Archive
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ScmBranch
Specific branch, tag or commit to checkout.

```yaml
Type: String
Parameter Sets: Git, Svn
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
Type: SwitchParameter
Parameter Sets: Git, Svn, Insights, Archive
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
Type: SwitchParameter
Parameter Sets: Git, Svn, Insights, Archive
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
Parameter Sets: Git
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
Type: SwitchParameter
Parameter Sets: Git
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ScmUpdateOnLaunch
Update the project when a job is launched that uses the project.

```yaml
Type: SwitchParameter
Parameter Sets: Git, Svn, Insights, Archive
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
Parameter Sets: Git, Svn, Archive
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SignatureValidationCredential
An optional credential used for validating files in the project against unexpected changes.

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

### -Subversion
Create a "svn" (Subversion) type project.

```yaml
Type: SwitchParameter
Parameter Sets: Svn
Aliases:

Required: True
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

### None
## OUTPUTS

### Jagabata.Resources.Project
New created Project object.

## NOTES

## RELATED LINKS

[Get-AnsibleProject](Get-AnsibleProject.md)

[Find-AnsibleProject](Find-AnsibleProject.md)

[Update-AnsibleProject](Update-AnsibleProject.md)

[Remove-AnsibleProject](Remove-AnsibleProject.md)
